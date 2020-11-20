using ScriptableObjects.EnemiesSystem;
using UnityEngine;

namespace MonoBehaviours
{
    public class EnemyController : MonoBehaviour
    {
    
        #region Interface Variables

        [SerializeField] private EnemyBehaviourData enemyBehaviourData;
        [SerializeField] private float jumpForce = 300;

        #endregion
    
        #region Private Variables

        private GameObject _player;
        private Vector3 _lastPosition;
        private float _timer = 2;
        
        #endregion

        void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }
    
        void Update()
        {
            enemyBehaviourData.enemyMovementStrategy.Move(gameObject.transform, _player.transform);
            _timer -= Time.deltaTime;
            CheckIfStuck();
            IsFacingPlayer();
            _lastPosition = gameObject.transform.position;
        }

        private void IsFacingPlayer()
        {
            var emuPosition = gameObject.transform.position;
            var playerPosition = _player.transform.position;

            gameObject.GetComponent<SpriteRenderer>().flipX = (emuPosition.x - playerPosition.x < 0);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            
            if (col.gameObject.CompareTag("Projectile"))
            {
                Destroy(col.gameObject);
                Destroy(gameObject);
            }
            enemyBehaviourData.enemyAttackStrategy.Attack(col, enemyBehaviourData.damageGiven);
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
