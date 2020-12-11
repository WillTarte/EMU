using EnemySystem.ScriptableObjects;
using EnemySystem.ScriptableObjects.EnemyAttackStrategies;
using EnemySystem.ScriptableObjects.EnemyMovementStrategies;
using GameSystem;
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
        [SerializeField] float startBattleRange;

        #endregion

        #region Private Variables
    
        private GameObject _player;
        private bool _startBattle = false;
        private EnemyAttackStrategy _attackStrategy;
        private EnemyMovementStrategy _movementStrategy;
        private bool _isMoving = false;

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
            _isMoving = _movementStrategy.Move(gameObject.transform, _player.transform);
            IsFacingPlayer();
            _attackStrategy.Attack(_player, gameObject, enemyBehaviourData.damageGiven);
        }

        public void ReceiveDamage(int amount)
        {
            //CyborgEmu can't receive damage while moving
            if (_isMoving && gameObject.name == "CyborgEmu")
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

        public bool isMoving()
        {
            return _isMoving;
        }

        private void IsFacingPlayer()
        {
            //Only Babe shouldn't flip while moving
            if (_isMoving && gameObject.name == "Babe")
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
            if (gameObject.name == "Babe")
            {
                if (Vector2.Distance(gameObject.transform.position, _player.transform.position) < startBattleRange && !_startBattle)
                {
                    _startBattle = true;
                    UpdateBossHealthBarHUD(healthPoints);
                }
            }
            else
            {
                if (Mathf.Abs(gameObject.transform.position.x - _player.transform.position.x) < startBattleRange && 
                    Mathf.Abs(gameObject.transform.position.y - _player.transform.position.y) < 0.5 && 
                    !_startBattle)
                {
                    GameObject.Find("Grid").transform.GetChild(0).gameObject.SetActive(true);
                    _startBattle = true;
                    UpdateBossHealthBarHUD(healthPoints);
                }
            }

        }

        private void LevelTransition()
        {
            if (gameObject.name == "Babe")
            {
                Destroy(gameObject);
                Indestructibles.LastLevel++;
                Indestructibles.respawnPos = Indestructibles.defaultSpawns[Indestructibles.LastLevel - 1];
                SceneManager.LoadScene(Indestructibles.LastLevel);
            }
        }

    }
}