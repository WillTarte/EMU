using Player.States;
using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        private BaseState _currentState;

        private bool _isFacingRight = true;

        private bool _shouldRoll = false;
        private bool _shouldJump = false;

        public Rigidbody2D Rigidbody { get; private set; }
        public Animator Animator { get; private set; }

        public float speed = 5.0F;
        public float jumpForce = 500.0F;

        // Start is called before the first frame update
        void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            
            ChangeState(new IdleState());
        }

        // Update is called once per frame
        void Update()
        {
            _currentState?.Update();
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
    }
}