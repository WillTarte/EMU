using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    [CreateAssetMenu(fileName = "KeepDistanceStrategy",
        menuName = "ScriptableObjects/EnemyMovement/KeepDistance", order = 3)]
    public class KeepDistanceStrategy : EnemyMovementStrategy
    {
        #region Interface Variables

        [SerializeField] private float minDistanceBuffer = 10;
        [SerializeField] private float speed = 3;

        #endregion

        public override void Move(Transform emuTransform, Transform playerTransform)
        {
            if (Vector2.Distance(emuTransform.position, playerTransform.position) < minDistanceBuffer)
            {
                emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                
                Vector3 movementDirection = new Vector3(emuTransform.position.x, 0, 0) -
                                            new Vector3(playerTransform.position.x, 0, 0);
                emuTransform.Translate(movementDirection.normalized * speed * Time.deltaTime);
            }
            else
            {
                emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
            }
        }
    }
}