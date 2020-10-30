using System.Collections;
using System.Collections.Generic;
using Player.States;
using UnityEngine;
using Controller = Player.Controller;

namespace Player.States
{
    public class IdleState : BaseState
    {
        public override void Start()
        {
            base.Start();
            
            Debug.Log("Idle State");
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                base.Controller.ChangeState(new RunState {IsFacingRight = true});
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                base.Controller.ChangeState(new RunState {IsFacingRight = false});
            }

            base.Controller.Animator.SetInteger("AnimState", 0);
            base.Controller.Animator.SetBool("Grounded", true);
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }   
}
