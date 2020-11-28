using Player.Commands;
using UnityEngine;

namespace Player.States
{
    /// <summary>
    /// The State where the player is Falling
    /// </summary>
    public class FallState : BaseState
    {
        public override void Start()
        {
            base.Start();
        }

        public override void Update(Command cmd)
        {
            base.Update(cmd);

            if (cmd is ShootCommand || cmd is ReloadCommand || cmd is SwitchWeaponCommand || cmd is ThrowCommand
                || cmd is LeaveGameCommand)
            {
                cmd.Execute(Controller);
            }

            Controller.UpdateTextureDirection();

            if (Controller.IsGrounded)
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

            Controller.MoveX(Input.GetAxisRaw("Horizontal"));

            var fallMultiplier = 1.0f + (Controller.fallMultiplier / 100.0f);
            Controller.Rigidbody.velocity += Vector2.up * (Physics2D.gravity.y * fallMultiplier * Time.deltaTime);
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}