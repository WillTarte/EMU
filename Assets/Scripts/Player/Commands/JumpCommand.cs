using UnityEngine;

namespace Player.Commands
{
    public class JumpCommand : Command
    {
        public override void Execute(Controller controller)
        {
            base.Execute(controller);

            controller.Animator.SetTrigger("Jump");
            controller.Rigidbody.AddForce(new Vector2(0.0F, controller.jumpForce));
        }
    }   
}
