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

        private bool firstMove = true;
        private int jumpRange = 4;
        private float startMoveTime = 0.0f;
        private float endMoveTime = 0.0f;
        private bool isCharging = false;
        private bool isJumping = false;
        private Vector2 direction;
        private Vector2 whereToJump;

        #endregion

        public override bool Move(Transform emuTransform, Transform playerTransform)
        {
            ResetValuesAtStart();
            
            if (playerTransform != null && emuTransform.gameObject.GetComponent<BossController>().battleStarted())
            {
                if (startMoveTime < Time.time)
                {
                
                    if (PlayerIsOnPlatformAbove(emuTransform, playerTransform) && !isCharging)
                    {
                        JumpToPlayer(emuTransform, playerTransform);
                    }
                    else if (!isJumping)
                    {
                        ChargeTowardsPlayer(emuTransform, playerTransform);
                    }

                    BringBabeDown(emuTransform);
                    
                    if (endMoveTime <= startMoveTime)
                    {
                        endMoveTime = startMoveTime + chargeTime;
                    }
                }
                
                if (endMoveTime <= startMoveTime)
                    endMoveTime = startMoveTime + chargeTime;
                else if (endMoveTime < Time.time)
                {
                    isCharging = false;
                    isJumping = false;
                    startMoveTime = Time.time + waitTime;
                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
                    emuTransform.GetComponent<BoxCollider2D>().enabled = true;
                    emuTransform.GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
            return (isCharging || isJumping);
        }

        private bool PlayerIsOnPlatformAbove(Transform emuTransform, Transform playerTransform)
        {
            return Mathf.Abs(emuTransform.position.x - playerTransform.position.x) < chargeRange &&
                   (Mathf.Abs(emuTransform.position.y) - Mathf.Abs(playerTransform.position.y)) > jumpRange;
        }

        private void ResetValuesAtStart()
        {
            if (Time.time < 0.1f)
            {
                startMoveTime = 0.0f;
                endMoveTime = 0.0f;
                isCharging = false;
                direction = Vector2.zero;
                whereToJump = Vector2.zero;
                firstMove = false;
            }
        }

        private void JumpToPlayer(Transform emuTransform, Transform playerTransform)
        {
                if (!isJumping)
                {
                    whereToJump = new Vector2(playerTransform.position.x, emuTransform.position.y);
                    isJumping = true;
                }

                if (whereToJump != Vector2.zero)
                {
                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                    emuTransform.position =
                        Vector2.MoveTowards(emuTransform.position, whereToJump, chargeSpeed * 1.5f * Time.deltaTime);
                }


                if (Vector2.Distance(emuTransform.position,whereToJump) == 0)
                {
                    whereToJump = Vector2.zero;
                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
                    emuTransform.GetComponent<BoxCollider2D>().enabled = false;
                    emuTransform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 1200));
                }
        }

        private void BringBabeDown(Transform emuTransform)
        {
            if (emuTransform.position.y > -10)
            {
                emuTransform.GetComponent<Rigidbody2D>().gravityScale = 10;
                emuTransform.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        private void ChargeTowardsPlayer(Transform emuTransform, Transform playerTransform)
        {
            if (!isCharging)
            {
                direction = (new Vector2(playerTransform.position.x, 0) -
                             new Vector2(emuTransform.position.x, 0));

                isCharging = true;
            }

            emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);

            emuTransform.Translate(direction.normalized * chargeSpeed * Time.deltaTime);
        }

    }
}