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

            if (cmd is JumpCommand || cmd is PickupCommand || cmd is ShootCommand || cmd is ReloadCommand || cmd is SwitchWeaponCommand)
            {
                cmd.Execute(Controller);
            }

            Controller.UpdateTextureDirection();
            Controller.MoveX(Input.GetAxisRaw("Horizontal"));

            if ((!Controller.IsPressingLeft && !Controller.IsPressingRight) ||
                (Controller.IsPressingLeft && Controller.IsPressingRight))
            {
                Controller.ChangeState(new IdleState());
            }

            if (Controller.IsPressingUp || Controller.IsPressingDown)
            {
                if (Controller.CanClimb)
                {
                    Controller.ChangeState(new ClimbState());
                }
            }

            if (!Controller.IsGrounded)
            {
                Controller.ChangeState(new FallState());
            }

            Controller.CheckForRoll();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}