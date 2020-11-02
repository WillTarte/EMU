using System;
using Player.States;
using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        #region Private Variables
        
        private BaseState _currentState;
        private InputHandler _inputHandler;
        
        #endregion

        #region Interface Variables
        
        public Rigidbody2D Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        
        public bool IsGrounded { get; private set; }
        public bool IsFacingRight { get; private set; } // TODO: Should also flip weapons
        
        #endregion

        #region Public variables
        
        public float speed = 5.0F;
        public float jumpForce = 400.0F;

        #endregion
        
        #region Private Methods
        
        // Start is called before the first frame update
        void Start()
        {
            _inputHandler = new InputHandler();
            
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();

            IsGrounded = false;
            IsFacingRight = true;
            
            ChangeState(new IdleState());
        }

        // Update is called once per frame
        void Update()
        {
            _currentState?.Update(_inputHandler.HandleInput());
        }

        private void FlipSprite()
        {
            IsFacingRight = !IsFacingRight;

            var scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Set the direction of the texture based on keyboard input
        /// </summary>
        public void UpdateTextureDirection()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!IsFacingRight)
                {
                    FlipSprite();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (IsFacingRight)
                {
                    FlipSprite();
                }
            }
        }
        
        public void Move(float fixedSpeed)
        {
            float moveBy = fixedSpeed * speed;
            Rigidbody.velocity = new Vector2(moveBy, Rigidbody.velocity.y);
        }

        public void ResetSpriteFlip()
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);

            if (!IsFacingRight)
            {
                scale.x *= -1;
            }

            transform.localScale = scale;
        }

        public void CheckForRoll()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (IsGrounded)
                {
                    ChangeState(new RollState {XAxisRaw = Input.GetAxisRaw("Horizontal")});
                }
            }
        }

        /// <summary>
        /// Method used to change state
        /// </summary>
        /// <param name="newState">New state.</param>
        public void ChangeState(BaseState newState)
        {
            _currentState?.Destroy();

            _currentState = newState;
            
            if (_currentState != null)
            {
                _currentState.Controller = this;
                _currentState.Start();
            }
        }

        #endregion
        
        #region Event Methods
        
        /// <summary>
        /// Event function called at the end of the Roll animation
        /// </summary>
        private void OnRollEnd()
        {
            Debug.Log("On Roll End");
            if (Input.GetKey(KeyCode.RightArrow))
            {
                IsFacingRight = true;
                ChangeState(new RunState());
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                IsFacingRight = false;
                ChangeState(new RunState());
            }
            else
            { 
                ChangeState(new IdleState());
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Ground hit");
                IsGrounded = true;   
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Ground left");
                IsGrounded = false;   
            }
        }
        
        #endregion
    }
}