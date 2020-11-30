using System.Runtime.InteropServices;
using EnemySystem.ScriptableObjects;
using EnemySystem.ScriptableObjects.EnemyAttackStrategies;
using EnemySystem.ScriptableObjects.EnemyMovementStrategies;
using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace EnemySystem.Monobehaviours
{
    public class BabeController : MonoBehaviour
    {
        #region Interface Variables
        
        [SerializeField] public int healthPoints;
        [SerializeField] private EnemyBehaviourData enemyBehaviourData;
        [SerializeField] private float jumpForce = 300;
        [SerializeField] private GameObject bloodEffect;

        #endregion

        #region Private Variables

        private GameObject _player;
        private Vector3 _lastPosition;
        private float _timer = 2;
        private bool _gotHit = false;
        private EnemyAttackStrategy _attackStrategy;
        private EnemyMovementStrategy _movementStrategy;
        private bool isMoving = false;

        #endregion

        void Start()
        {
            _attackStrategy = enemyBehaviourData.enemyAttackStrategy;
            _movementStrategy = enemyBehaviourData.enemyMovementStrategy;
            _player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            isMoving = _movementStrategy.Move(gameObject.transform, _player.transform);
            _timer -= Time.deltaTime;
            IsFacingPlayer();
            _attackStrategy.Attack(_player, gameObject, enemyBehaviourData.damageGiven);
            _lastPosition = gameObject.transform.position;
        }

        public void ReceiveDamage(int amount)
        {
            healthPoints -= amount;
            _gotHit = true;
            if (healthPoints <= 0)
            {
                var blood = Instantiate(bloodEffect, transform.position, transform.rotation);
                Destroy(blood, 0.51f);
                Destroy(gameObject);
            }
        }

        public bool gotHit()
        {
            return _gotHit;
        }

        private void IsFacingPlayer()
        {
            if (!isMoving)
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
        }

    }
}