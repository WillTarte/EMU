using UnityEngine;

namespace Player.States
{
    /// <summary>
    /// The State where the player is Jumping
    /// </summary>
    public class JumpState : BaseState
    {
        public override void Start()
        {
            base.Start();
            
            Debug.Log("Jump State");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }   
}