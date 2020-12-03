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
        [SerializeField] private float chargeTime;
        [SerializeField] private float waitTime;

        #endregion

        #region Private Variables

        private float startMoveTime = 0.0f;
        private float endMoveTime = 0.0f;
        private bool isCharging = false;
        private Vector2 direction;

        #endregion

        public override bool Move(Transform emuTransform, Transform playerTransform)
        {
            ResetValuesAtStart();
            if (playerTransform != null &&
                (Vector2.Distance(emuTransform.position, playerTransform.position) < chargeRange ||
                 emuTransform.gameObject.GetComponent<BossController>().battleStarted()))
            {
                if (startMoveTime < Time.time || isCharging)
                {
                    if (!isCharging)
                    {
                        direction = (new Vector2(playerTransform.position.x, 0) -
                                     new Vector2(emuTransform.position.x, 0));

                        isCharging = true;
                    }

                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);

                    emuTransform.Translate(direction.normalized * chargeSpeed * Time.deltaTime);

                    if (endMoveTime <= startMoveTime)
                    {
                        endMoveTime = startMoveTime + chargeTime;
                    }
                }

                if (endMoveTime < Time.time && isCharging)
                {
                    isCharging = false;
                    startMoveTime = Time.time + waitTime;
                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
                }
            }
            return isCharging;
        }

        private void ResetValuesAtStart()
        {
            if (Time.time < 0.2f)
            {
                startMoveTime = 0.0f;
                endMoveTime = 0.0f;
                isCharging = false;
                direction = Vector2.zero;
            }
        }

        private void JumpToPlayerPlatform()
        {
            
        }
    }
}