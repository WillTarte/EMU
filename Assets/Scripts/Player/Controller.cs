using System.Collections;
using System.Collections.Generic;
using Player.States;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Serialization;

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

        void HandleInput()
        {
            _shouldRoll = Input.GetKeyDown(KeyCode.LeftShift);
            _shouldJump = Input.GetKeyDown(KeyCode.Space);
        }

        void Flip()
        {
            _isFacingRight = !_isFacingRight;

            var scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }

        void SetAnimation()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (_shouldRoll)
                {
                    Animator.SetTrigger("Roll");
                }
                else if (_shouldJump)
                {
                    Animator.SetTrigger("Jump");
                }
                else
                {
                    Animator.SetInteger("AnimState", 1);
                }

                if (!_isFacingRight)
                {
                    Flip();
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (_shouldRoll)
                {
                    Animator.SetTrigger("Roll");
                }
                else if (_shouldJump)
                {
                    Animator.SetTrigger("Jump");
                }
                else
                {
                    Animator.SetInteger("AnimState", 1);
                }

                if (_isFacingRight)
                {
                    Flip();
                }
            }
            else
            {
                if (_shouldJump)
                {
                    Animator.SetTrigger("Jump");
                }
                else
                {
                    Animator.SetInteger("AnimState", 0);
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

        /// <summary>
        /// Event function called at the end of the Roll animation
        /// </summary>
        void OnRollEnd()
        {
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