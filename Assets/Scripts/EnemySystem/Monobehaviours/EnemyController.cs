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
        private float _timer = 3;
        private bool _gotHit;
        private EnemyAttackStrategy _attackStrategy;
        private EnemyMovementStrategy _movementStrategy;
        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private bool _killed;
        private Rigidbody2D _rigidbody;
        private bool Grounded => _rigidbody.velocity.y == 0;

        #endregion

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _attackStrategy = enemyBehaviourData.enemyAttackStrategy;
            _movementStrategy = enemyBehaviourData.enemyMovementStrategy;
            _player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            _timer -= Time.deltaTime;
            _lastPosition = gameObject.transform.position;
            _movementStrategy.Move(gameObject.transform, _player.transform);
            CheckIfStuck();
            IsFacingPlayer();
            _attackStrategy.Attack(_player, gameObject, enemyBehaviourData.damageGiven);
        }

        public void ReceiveDamage(int amount)
        {
            if (_killed) return;
            healthPoints -= amount;
            _gotHit = true;
            if (healthPoints <= 0)
            {
                _killed = true;
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

        /**
         * Make the emu jump if it hits an object on the x-axis
         */
        private void OnCollisionStay2D(Collision2D other)
        {
            if (Grounded)
            {
                JumpOverObstacle(other);
            }
        }

        private void JumpOverObstacle(Collision2D other)
        {
            foreach (ContactPoint2D point2D in other.contacts)
            {
                if (!other.gameObject.CompareTag("Player")
                    && point2D.normal.x > 0.5f
                    || point2D.normal.x < -0.5f)
                {
                    _rigidbody.AddForce(new Vector2(0.0f, jumpForce));
                    break;
                }
            }
        }

        private void CheckIfStuck()
        {
            if (_timer < 0
                && Vector3.Distance(_lastPosition, gameObject.transform.position) < 0.05
                && Vector3.Distance(_lastPosition, gameObject.transform.position) > 0.0 && Grounded)
            {
                _rigidbody.AddForce(new Vector2(0.0f, jumpForce));
                _timer = 3;
            }
        }
    }
}