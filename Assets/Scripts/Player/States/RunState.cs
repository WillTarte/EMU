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
            Controller.Move(Input.GetAxisRaw("Horizontal"));
            
            if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                Controller.ChangeState(new IdleState());
            }
            
            Controller.CheckForRoll();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}