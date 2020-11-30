using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    public interface IEnemyMovementStrategy
    {
        bool Move(Transform emuTransform, Transform playerTransform);
    }
}
