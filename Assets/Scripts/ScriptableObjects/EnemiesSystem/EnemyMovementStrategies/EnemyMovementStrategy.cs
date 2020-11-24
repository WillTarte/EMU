using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyMovementStrategies
{
    public abstract class EnemyMovementStrategy : ScriptableObject, IEnemyMovementStrategy
    {
        public abstract void Move(Transform emuTransform, Transform playerTransform);
    }
}
