using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller = Player.Controller;

namespace Player.States
{
    public abstract class BaseState
    {
        public Controller Controller;

        /// <summary>
        /// Method called to setup all data for the state - same as MonoBehaviour's Start()
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Method called to update the state - same as MonoBehaviour's Update()
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Method called to cleanup the state
        /// </summary>
        public virtual void Destroy()
        {
        }
    }
}