using System;
using UnityEngine;

namespace ScriptableObjects.WeaponsSystem.WeaponShootStrategies
{
    public class WaitForAndWhile : CustomYieldInstruction
    {
        private readonly Func<bool> _predicate;
        private bool _condition;
        private float _timer;

        public WaitForAndWhile(Func<bool> predicate, float timeToWait)
        {
            _predicate = predicate;
            _condition = false;
            _timer = timeToWait;
        }
        
        public override bool keepWaiting
        {
            get
            {
                _timer -= Time.deltaTime;
                if (_predicate())
                {
                    _condition = true;
                }

                return !_condition || _timer > 0.0f;
            }
        }
    }
}