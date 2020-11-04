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
            
            Controller.Rigidbody.gravityScale = 1.0F;
        }

        /// <summary>
        /// Method called to update the state - same as MonoBehaviour's Update()
        /// </summary>
        public virtual void Update(Command cmd)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Passthrough"), Controller.Rigidbody.velocity.y > 0.0F);

            Controller.Animator.SetFloat("AirSpeedY", Controller.Rigidbody.velocity.y);
            Controller.Animator.SetBool("Grounded", Controller.IsGrounded);
        }

        /// <summary>
        /// Method called to cleanup the state
        /// </summary>
        public virtual void Destroy()
        {
        }
    }
}