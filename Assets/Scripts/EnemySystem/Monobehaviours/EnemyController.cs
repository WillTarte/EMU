using System;
using System.Collections;
using EnemySystem.ScriptableObjects;
using EnemySystem.ScriptableObjects.EnemyAttackStrategies;
using EnemySystem.ScriptableObjects.EnemyMovementStrategies;
using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace EnemySystem.Monobehaviours
{
    public class EnemyController : MonoBehaviour
    {
        #region Interface Variables
        
        [SerializeField] public int healthPoints;
        [SerializeField] private EnemyBehaviourData enemyBehaviourData;
        [SerializeField] private float jumpForce = 300;
        [SerializeField] private GameObject bloodEffect;
        [SerializeField] private AudioClip duckDie;

        #endregion

        #region Private Variables

        private GameObject _player;
        private Vector3 _lastPosition;
        private float _timer = 2;
        private bool _hasCollided;
        private bool _gotHit = false;
        private EnemyAttackStrategy _attackStrategy;
        private EnemyMovementStrategy _movementStrategy;
        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        #endregion

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            _attackStrategy = enemyBehaviourData.enemyAttackStrategy;
            _movementStrategy = enemyBehaviourData.enemyMovementStrategy;
            _player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            _movementStrategy.Move(gameObject.transform, _player.transform);
            _timer -= Time.deltaTime;
            CheckIfStuck();
            IsFacingPlayer();
            _hasCollided = _attackStrategy.Attack(_player, gameObject, enemyBehaviourData.damageGiven, _hasCollided);
            _lastPosition = gameObject.transform.position;
        }

        public void ReceiveDamage(int amount)
        {
            healthPoints -= amount;
            _gotHit = true;
            if (healthPoints <= 0)
            {
                StartCoroutine(Killed());
            }
        }

        private IEnumerator Killed()
        {
            _animator.enabled = false;
            enabled = false;
            _spriteRenderer.sprite = null;
            var blood = Instantiate(bloodEffect, transform.position, transform.rotation);
            _audioSource.PlayOneShot(duckDie, PlayerPrefs.GetInt("volume") / 10.0f);
            yield return new WaitWhile(() => _audioSource.isPlaying);
            Destroy(blood);
            Destroy(gameObject);
        }

        public bool GotHit()
        {
            return _gotHit;
        }

        private void IsFacingPlayer()
        {
            var emuPosition = gameObject.transform.position;
            var playerPosition = _player.transform.position;

            gameObject.GetComponent<SpriteRenderer>().flipX = (emuPosition.x - playerPosition.x < 0);
            if (gameObject.GetComponentInChildren<WeaponBehaviourScript>() != null)
            {
                gameObject.GetComponentInChildren<WeaponBehaviourScript>().Direction =
                    gameObject.GetComponent<SpriteRenderer>().flipX ? Vector2.right : Vector2.left;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                _hasCollided = true;
            }

            //enemyBehaviourData.enemyAttackStrategy.Attack(col, enemyBehaviourData.damageGiven);
        }

        /**
         * Make the emu jump if it hits an object on the x-axis
         */
        private void OnCollisionEnter2D(Collision2D other)
        {
            JumpOverObstacle(other);
        }

        private void JumpOverObstacle(Collision2D other)
        {
            foreach (ContactPoint2D point2D in other.contacts)
            {
                if (!other.gameObject.CompareTag("Player")
                    && point2D.normal.x > 0.5f
                    || point2D.normal.x < -0.5f)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, jumpForce));
                    break;
                }
            }
        }

        private void CheckIfStuck()
        {
            if (_timer < 0
                && Vector3.Distance(_lastPosition, gameObject.transform.position) < 0.05
                && Vector3.Distance(_lastPosition, gameObject.transform.position) > 0.0)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, jumpForce));
                _timer = 2;
            }
        }
    }
}