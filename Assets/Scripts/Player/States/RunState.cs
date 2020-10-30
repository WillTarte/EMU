using UnityEngine;

namespace Player.States
{
    /// <summary>
    /// The State where the player is either moving left or right
    /// </summary>
    public class RunState : BaseState
    {
        public bool IsFacingRight = true;

        public override void Start()
        {
            base.Start();

            Debug.Log("Run State");

            var scale = Controller.transform.localScale;
            scale.x = Mathf.Abs(scale.x);

            if (!IsFacingRight)
            {
                scale.x *= -1;
            }

            Controller.transform.localScale = scale;
        }

        public override void Update()
        {
            base.Update();

            UpdateTextureDirection();
            
            if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                Controller.ChangeState(new IdleState());
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Controller.ChangeState(new RollState{XAxisRaw = Input.GetAxisRaw("Horizontal")});
            }

            Controller.Animator.SetInteger("AnimState", 1);

            Move();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        /// <summary>
        /// Move the character a fixed amount per frame based on user input
        /// </summary>
        void Move()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float moveBy = x * Controller.speed;
            Controller.Rigidbody.velocity = new Vector2(moveBy, Controller.Rigidbody.velocity.y);
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

            var scale = Controller.transform.localScale;
            scale.x *= -1;

            Controller.transform.localScale = scale;
        }
    }
}