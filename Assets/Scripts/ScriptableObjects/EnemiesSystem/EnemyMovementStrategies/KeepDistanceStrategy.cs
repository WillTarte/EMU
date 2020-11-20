using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyMovementStrategies
{
    [CreateAssetMenu(fileName = "KeepDistanceStrategy",
        menuName = "ScriptableObjects/EnemyMovement/KeepDistance", order = 3)]
    public class KeepDistanceStrategy : EnemyMovementStrategy
    {
        #region Interface Variables

        [SerializeField] private float minDistanceBuffer = 10;
        [SerializeField] private float speed = 4;

        #endregion

        public override void Move(Transform emuTransform, Transform playerTransform)
        {
            if (Vector2.Distance(emuTransform.position, playerTransform.position) < minDistanceBuffer)
            {
                emuTransform.position =
                    Vector2.MoveTowards(emuTransform.position, new Vector2(-playerTransform.position.x, 0),
                        speed * Time.deltaTime);
            }
        }
    }
}