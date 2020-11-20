using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyMovementStrategies
{
    [CreateAssetMenu(fileName = "KeepDistanceStrategy", 
        menuName = "ScriptableObjects/EnemyMovement/KeepDistance", order = 3)]
    public class KeepDistanceStrategy : EnemyMovementStrategy
    {
        public override void Move(Transform emuTransform, Transform playerTransform)
        {
            throw new System.NotImplementedException();
        }
    }
}
