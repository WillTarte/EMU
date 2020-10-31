using Player.Commands;
using UnityEngine;

namespace Player.States
{
    /// <summary>
    /// The State where the player is either moving left or right
    /// </summary>
    public class RunState : BaseState
    {
        public override void Start()
        {
            base.Start();

            Debug.Log("Run State");

            Controller.Animator.SetInteger("AnimState", 1);
        }

        public override void Update(Command cmd)
        {
            base.Update(cmd);

            if (cmd is JumpCommand)
            {
                cmd?.Execute(Controller);
            }
            
            Controller.UpdateTextureDirection();
            
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

            Controller.Move(Input.GetAxisRaw("Horizontal"));
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}