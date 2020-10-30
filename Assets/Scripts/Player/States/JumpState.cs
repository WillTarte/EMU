using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.States
{
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