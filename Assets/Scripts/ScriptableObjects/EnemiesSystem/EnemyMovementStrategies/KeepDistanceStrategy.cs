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
                Vector3 movementDirection = new Vector3(emuTransform.position.x, 0, 0) -
                                            new Vector3(playerTransform.position.x, 0, 0);
                emuTransform.Translate(movementDirection.normalized * speed * Time.deltaTime);
            }
        }
    }
}