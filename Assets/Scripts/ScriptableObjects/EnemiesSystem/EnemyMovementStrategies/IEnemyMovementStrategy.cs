using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyMovementStrategies
{
    public interface IEnemyMovementStrategy
    {
        void Move(Transform emuTransform, Transform playerTransform);
    }
}
