using UnityEngine;

namespace Player.Commands
{
    public class JumpCommand : Command
    {
        public override void Execute(Controller controller)
        {
            base.Execute(controller);
        
            Debug.Log("Jump command");

            controller.Rigidbody.AddForce(new Vector2(0.0F, controller.jumpForce));
            controller.Animator.SetTrigger("Jump");
        }
    }   
}
