using System;
using Player.States;
using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        private BaseState _currentState;

        public InputHandler InputHandler { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public Animator Animator { get; private set; }

        public bool IsGrounded { get; private set; }

        public float speed = 5.0F;
        public float jumpForce = 400.0F;

        // Start is called before the first frame update
        void Start()
        {
            InputHandler = new InputHandler();
            
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();

            IsGrounded = false;
            
            ChangeState(new IdleState());
        }

        // Update is called once per frame
        void Update()
        {
            var command = InputHandler.HandleInput();
            
            _currentState?.Update(command);
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
        
        /// <summary>
        /// Event function called at the end of the Roll animation
        /// </summary>
        void OnRollEnd()
        {
            Debug.Log("On Roll End");
            if (Input.GetKey(KeyCode.RightArrow))
            {
                ChangeState(new RunState {IsFacingRight = true});
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                ChangeState(new RunState {IsFacingRight = false});
            }
            else
            { 
                ChangeState(new IdleState());
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            IsGrounded = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            IsGrounded = false;
        }
    }
}