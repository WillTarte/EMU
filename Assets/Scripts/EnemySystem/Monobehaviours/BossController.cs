using EnemySystem.ScriptableObjects;
using EnemySystem.ScriptableObjects.EnemyAttackStrategies;
using EnemySystem.ScriptableObjects.EnemyMovementStrategies;
using UnityEngine;
using UnityEngine.SceneManagement;
using WeaponsSystem.MonoBehaviours;

namespace EnemySystem.Monobehaviours
{
    public class BossController : MonoBehaviour
    {
        #region Interface Variables
        
        [SerializeField] private int healthPoints;
        [SerializeField] private EnemyBehaviourData enemyBehaviourData;

        #endregion

        #region Private Variables
    
        private GameObject _player;
        private bool _startBattle = false;
        private EnemyAttackStrategy _attackStrategy;
        private EnemyMovementStrategy _movementStrategy;
        private bool isMoving = false;
        private float startBattleRange = 30;

        #endregion

        #region Public Variables
        
        public delegate void UpdateHUDBossHealthBarHandler(int hitPoints);

        public event UpdateHUDBossHealthBarHandler UpdateBossHealthBarHUD;
        
        #endregion
        
        void Start()
        {
            _attackStrategy = enemyBehaviourData.enemyAttackStrategy;
            _movementStrategy = enemyBehaviourData.enemyMovementStrategy;
            _player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            StartBattle();
            isMoving = _movementStrategy.Move(gameObject.transform, _player.transform);
            IsFacingPlayer();
            _attackStrategy.Attack(_player, gameObject, enemyBehaviourData.damageGiven);
        }

        public void ReceiveDamage(int amount)
        {
            if (isMoving && gameObject.name == "CyborgEmu")
                return;
            healthPoints -= amount;
            UpdateBossHealthBarHUD(healthPoints);
            if (healthPoints <= 0)
            {
                // [ insert narration here ]
                LevelTransition();
            }
        }

        public bool battleStarted()
        {
            return _startBattle;
        }

        private void IsFacingPlayer()
        {
            if (isMoving && gameObject.name != "CyborgEmu")
                return;
            else
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

        private void StartBattle()
        {
            if (Vector2.Distance(gameObject.transform.position, _player.transform.position) < startBattleRange && !_startBattle)
            {
                _startBattle = true;
                UpdateBossHealthBarHUD(healthPoints);
            }
        }

        private void LevelTransition()
        {
            if (gameObject.name == "Babe")
            {
                Destroy(gameObject);
                Indestructibles.LastLevel = 2;
                SceneManager.LoadScene(Indestructibles.LastLevel);
            }
        }

    }
}