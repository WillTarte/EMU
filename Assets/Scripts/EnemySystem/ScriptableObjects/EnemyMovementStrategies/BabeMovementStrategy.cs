using EnemySystem.Monobehaviours;
using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    [CreateAssetMenu(fileName = "BabeMovementStrategy",
        menuName = "ScriptableObjects/EnemyMovement/BabeMovement", order = 3)]
    public class BabeMovementStrategy : EnemyMovementStrategy
    {
        #region Interface Variables

        [SerializeField] private float chargeSpeed;
        [SerializeField] private float chargeRange;
        [SerializeField] private float threshold;
        [SerializeField] private float chargeTime;
        [SerializeField] private float waitTime;

        #endregion

        #region Private Variables

        private float startMoveTimer = 0.0f;
        private float endMoveTimer = 0.0f;
        private bool isCharging = false;
        private Vector2 direction;

        #endregion

        public override void Move(Transform emuTransform, Transform playerTransform)
        {
            if (Time.time < 1)
            {
                isCharging = false;
                startMoveTimer = 0.0f;
                endMoveTimer = 0.0f;
                direction = Vector2.zero;
            }
            if (playerTransform != null &&
                (Vector2.Distance(emuTransform.position, playerTransform.position) < chargeRange ||
                 emuTransform.gameObject.GetComponent<EnemyController>().gotHit()))
            {
                if (startMoveTimer < Time.time || isCharging)
                {
                    if (!isCharging)
                    {
                        direction = (new Vector2(playerTransform.position.x, 0) -
                                             new Vector2(emuTransform.position.x, 0));
                        
                        isCharging = true;
                    }

                    //emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                    
                    emuTransform.Translate(direction.normalized * chargeSpeed * Time.deltaTime);
                    
                    if (endMoveTimer <= startMoveTimer)
                    {
                        endMoveTimer = startMoveTimer + chargeTime;
                    }
                }

                if (endMoveTimer < Time.time && isCharging)
                {
                    isCharging = false;
                    startMoveTimer = Time.time + waitTime;
                    //emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
                }
            }
        }
    }
}