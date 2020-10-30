using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.States
{
    public class RollState : BaseState
    {
        public float XAxisRaw = 0.0f;
        
        public override void Start()
        {
            base.Start();
            
            Debug.Log("Roll State");
            
            Controller.Animator.SetTrigger("Roll");
        }

        public override void Update()
        {
            base.Update();

            Move();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
        
        void Move()
        {
            float moveBy = XAxisRaw * Controller.speed;
            Controller.Rigidbody.velocity = new Vector2(moveBy, Controller.Rigidbody.velocity.y);
        }
    }   
}
