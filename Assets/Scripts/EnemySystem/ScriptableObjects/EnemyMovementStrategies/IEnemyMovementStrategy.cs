using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    public interface IEnemyMovementStrategy
    {
        void Move(Transform emuTransform, Transform playerTransform);
    }
}
