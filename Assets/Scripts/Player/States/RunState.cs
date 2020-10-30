using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Player.States
{
    public class RunState : BaseState
    {
        public bool IsFacingRight = true;

        public override void Start()
        {
            base.Start();

            Debug.Log("Run State");

            var scale = base.Controller.transform.localScale;
            scale.x = Mathf.Abs(scale.x);

            if (!IsFacingRight)
            {
                scale.x *= -1;
            }

            base.Controller.transform.localScale = scale;
        }

        public override void Update()
        {
            base.Update();

            UpdateTextureDirection();
            
            if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                base.Controller.ChangeState(new IdleState());
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                base.Controller.Animator.SetTrigger("Roll");
                // base.Controller.ChangeState(new RollState());
            }

            base.Controller.Animator.SetInteger("AnimState", 1);

            Move();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        void Move()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float moveBy = x * base.Controller.speed;
            base.Controller.Rigidbody.velocity = new Vector2(moveBy, base.Controller.Rigidbody.velocity.y);
        }

        /// <summary>
        /// Set the direction of the texture based on keyboard input
        /// </summary>
        void UpdateTextureDirection()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!IsFacingRight)
                {
                    Flip();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (IsFacingRight)
                {
                    Flip();
                }
            }
        }

        /// <summary>
        /// Flip the texture around the y axis
        /// </summary>
        void Flip()
        {
            IsFacingRight = !IsFacingRight;

            var scale = base.Controller.transform.localScale;
            scale.x *= -1;

            base.Controller.transform.localScale = scale;
        }
    }
}