using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyMovementStrategies
{
    [CreateAssetMenu(fileName = "NewFollowTargetStrategy", 
        menuName = "ScriptableObjects/EnemyMovement/Follow", order = 3)]
    public class FollowTargetStrategy : EnemyMovementStrategy
    {
        
        #region Interface Variables

        [SerializeField] private float followSpeed = 4;
        [SerializeField] private float followRange = 10;
        
        #endregion
        
        public override void Move(Transform emuTransform, Transform playerTransform)
        {
            if (playerTransform != null && Vector2.Distance(emuTransform.position, playerTransform.position) < followRange)
            {
                emuTransform.position =
                    Vector2.MoveTowards(emuTransform.position, playerTransform.position, followSpeed * Time.deltaTime);
            }
        }
        
        
    }
}
