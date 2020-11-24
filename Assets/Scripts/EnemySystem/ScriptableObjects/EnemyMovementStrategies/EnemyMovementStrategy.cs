using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    public abstract class EnemyMovementStrategy : ScriptableObject, IEnemyMovementStrategy
    {
        public abstract void Move(Transform emuTransform, Transform playerTransform);
    }
}
