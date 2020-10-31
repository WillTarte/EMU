using Player.Commands;
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
            
            Controller.Animator.SetInteger("AnimState", 1);
        }

        public override void Update(Command cmd)
        {
            base.Update(cmd);

            if (cmd is JumpCommand)
            {
                cmd?.Execute(Controller);
            }
            
            UpdateTextureDirection();
            
            
            if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                Controller.Animator.SetInteger("AnimState", 0);
                
                Controller.ChangeState(new IdleState());
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Controller.Animator.SetInteger("AnimState", 0);
                
                Controller.ChangeState(new RollState{XAxisRaw = Input.GetAxisRaw("Horizontal")});
            }

            Move(Input.GetAxisRaw("Horizontal"));
        }

        public override void Destroy()
        {
            base.Destroy();
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