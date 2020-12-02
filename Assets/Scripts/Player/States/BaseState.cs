using Player.Commands;
using UnityEngine;

namespace Player.States
{
    /// <summary>
    /// Abstract class used to define an interface for all player states
    /// </summary>
    public abstract class BaseState
    {
        public Controller Controller;

        /// <summary>
        /// Method called to setup all data for the state - same as MonoBehaviour's Start()
        /// </summary>
        public virtual void Start()
        {
            Controller.ResetSpriteFlip();
            
            Controller.Animator.SetBool("Climbing", false);
            
            Controller.Rigidbody.gravityScale = 1.0F;
        }

        /// <summary>
        /// Method called to update the state - same as MonoBehaviour's Update()
        /// </summary>
        public virtual void Update(Command cmd)
        {
            bool shouldPassThru = (Controller.Rigidbody.velocity.y > 0.0f) || Controller.CanFallthrough;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Passthrough"), shouldPassThru);

            Controller.Animator.SetFloat("AirSpeedY", Controller.Rigidbody.velocity.y);
            Controller.Animator.SetBool("Grounded", Controller.IsGrounded);
        }

        /// <summary>
        /// Method called to cleanup the state
        /// </summary>
        public virtual void Destroy()
        {
        }
        
        /// <summary>
        /// Check if the character should start falling through platforms
        /// </summary>
        protected void CheckAndHandleFallthrough()
        {
            if (Controller.IsOnPlatform && Controller.IsPressingDown)
            {
                Controller.CanFallthrough = true;
                
                Controller.ChangeState(new FallState());
            }
        }

        /// <summary>
        /// Check and Handle whether the player should enter the falling state or not
        /// </summary>
        protected void CheckAndHandleFall()
        {
            if (!Controller.IsGrounded)
            {
                Controller.ChangeState(new FallState());
            }
        }
    }
}