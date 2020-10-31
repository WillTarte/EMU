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

            Debug.Log("Fall State");
        }

        public override void Update(Command cmd)
        {
            base.Update(cmd);

            if (Controller.IsGrounded)
            {
                Controller.ChangeState(new IdleState());
            }
            
            Move(Input.GetAxisRaw("Horizontal"));
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}