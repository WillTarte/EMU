using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    public abstract class EnemyMovementStrategy : ScriptableObject, IEnemyMovementStrategy
    {
        public abstract bool Move(Transform emuTransform, Transform playerTransform);
    }
}
