using System;
using UnityEngine;

namespace Enemy
{
    public class FollowTarget : MonoBehaviour
    {
        #region Interface Variables

        [SerializeField] private float mFollowSpeed;
        [SerializeField] private float mFollowRange;
        [SerializeField] private float mJumpForce;

        #endregion
        
        #region Private Variables
        
        private Vector3 _lastPosition;
        private float _timer = 2;
        private Transform _playerTransform;
        
        #endregion

        void Start()
        {
            _playerTransform = GameObject.FindWithTag("Player").transform;
            _lastPosition = gameObject.transform.position;
        }
        
        void Update()
        {
            FollowPlayer();
            _timer -= Time.deltaTime;
            CheckIfStuck();
            _lastPosition = gameObject.transform.position;
        }

        private void FollowPlayer()
        {
            if (_playerTransform != null && Vector2.Distance(transform.position, _playerTransform.position) < mFollowRange)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, _playerTransform.position, mFollowSpeed * Time.deltaTime);
            }
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
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, mJumpForce));
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
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, mJumpForce));
                _timer = 2;
            }
        }
    }
}