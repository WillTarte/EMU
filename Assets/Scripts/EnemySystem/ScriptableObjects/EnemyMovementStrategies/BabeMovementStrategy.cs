using EnemySystem.Monobehaviours;
using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    [CreateAssetMenu(fileName = "BabeMovementStrategy",
        menuName = "ScriptableObjects/EnemyMovement/BabeMovement", order = 3)]
    public class BabeMovementStrategy : EnemyMovementStrategy
    {
        #region Interface Variables
    
        //Atomic parameters of babe uwu
        [SerializeField] private float chargeSpeed;
        [SerializeField] private float jumpSpeedScale;
        [SerializeField] private float miniJumpForce;
        [SerializeField] private float superJumpForce;
        [SerializeField] private float attackTime;
        [SerializeField] private float waitTime;
        [SerializeField] private int jumpRange;
        [SerializeField] private float chargeJumpRangeX;
        [SerializeField] private float chargeJumpRangeY;
        [SerializeField] private float regularGravityScale;
        [SerializeField] private float bringDownGravityScale;
        [SerializeField] private float bringDownHeight;
        
        #endregion

        #region Private Variables

        //Variables used to define babe movements
        private bool firstMove = true;
        private bool chargeAttack = false;
        private bool jumpAttack = false;
        private bool chargeJumped = false;
        private bool hasPassedPlayer = false;
        
        //Variables used to specify when the attacks of babe starts or end
        private float startMoveTime;
        private float endMoveTime;
        
        //Variables used for babe movements
        private Vector2 direction;
        private Vector2 whereToJump;

        #endregion

        public override bool Move(Transform emuTransform, Transform playerTransform)
        {
            //Unity caches the values of Strategies variables
            //This is to make sure the variables are reset each time that you run the game in Unity
            #if UNITY_EDITOR
                ResetValuesAtStart();
            #endif
            
            if (playerTransform != null && emuTransform.gameObject.GetComponent<BossController>().battleStarted())
            {
                if (startMoveTime < Time.time)
                {
                    //If the player is on a platform, make Babe jump
                    if (PlayerIsOnPlatformAbove(emuTransform, playerTransform) && !chargeAttack || jumpAttack) 
                    {
                        JumpToPlayer(emuTransform, playerTransform);
                    }
                    //Otherwise, make Babe charge
                    else if (!jumpAttack)
                    {
                        ChargeTowardsPlayer(emuTransform, playerTransform);
                    }
                }
                
                //If Babe is too high in the sky, bring it back
                BringBabeDown(emuTransform);
                
                //If the timestamp of the new attack is defined, set the timestamp to end it
                if (endMoveTime <= startMoveTime)
                    endMoveTime = startMoveTime + attackTime;
                else if (endMoveTime <= Time.time)
                {
                    //Reset everything at the end of the attack
                    chargeAttack = false;
                    jumpAttack = false;
                    chargeJumped = false;
                    hasPassedPlayer = false;
                    startMoveTime = Time.time + waitTime;
                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
                    emuTransform.GetComponent<BoxCollider2D>().enabled = true;
                    emuTransform.GetComponent<Rigidbody2D>().gravityScale = regularGravityScale;
                }
            }
            return (chargeAttack || jumpAttack);
        }

        private void JumpToPlayer(Transform emuTransform, Transform playerTransform)
        {
                //If the Jump attack just started, set where Babe should jump
                if (!jumpAttack)
                {
                    whereToJump = new Vector2(playerTransform.position.x, emuTransform.position.y);
                    jumpAttack = true;
                }
                
                //Move Babe to whereToJump coordinates if need to
                if (whereToJump != Vector2.zero)
                {
                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                    emuTransform.position =
                        Vector2.MoveTowards(emuTransform.position, whereToJump, chargeSpeed * jumpSpeedScale * Time.deltaTime);
                }

                //When Babe reach her destination, make her jump
                if (Vector2.Distance(emuTransform.position,whereToJump) == 0)
                {
                    whereToJump = Vector2.zero;
                    emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
                    emuTransform.GetComponent<BoxCollider2D>().enabled = false;
                    emuTransform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, superJumpForce));
                }
        }

        private void ChargeTowardsPlayer(Transform emuTransform, Transform playerTransform)
        {
            //If Charge attack just started, set the direction where Babe should charge
            if (!chargeAttack)
            {
                direction = (new Vector2(playerTransform.position.x, 0) -
                             new Vector2(emuTransform.position.x, 0));
                if (direction == Vector2.zero)
                {
                    direction = Vector2.left;
                }
                chargeAttack = true;
            }
            
            //If the player is climbing, make Babe jump
            //This is to avoid the player to abuse of the climbing
            if (PlayerIsClimbing(emuTransform, playerTransform) && !chargeJumped && !hasPassedPlayer)
            {
                emuTransform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, miniJumpForce));
                chargeJumped = true;
            }

            emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
            
            emuTransform.Translate(direction.normalized * chargeSpeed * Time.deltaTime);
            
            //Avoid awkward jump after passing the player
            if (Mathf.Abs(emuTransform.position.x - playerTransform.position.x) < 0.2f)
            {
                hasPassedPlayer = true;
            }
        }

        private bool PlayerIsClimbing(Transform emuTransform, Transform playerTransform)
        {
            return Mathf.Abs(emuTransform.position.x - playerTransform.position.x) < chargeJumpRangeX &&
                   (Mathf.Abs(emuTransform.position.y) - Mathf.Abs(playerTransform.position.y)) > chargeJumpRangeY &&
                   (Mathf.Abs(emuTransform.position.y) - Mathf.Abs(playerTransform.position.y)) < jumpRange;
        }
        
        private bool PlayerIsOnPlatformAbove(Transform emuTransform, Transform playerTransform)
        {
            return (Mathf.Abs(emuTransform.position.y) - Mathf.Abs(playerTransform.position.y)) > jumpRange;
        }

        private void ResetValuesAtStart()
        {
            if (Time.time < 0.1f)
            {
                startMoveTime = 0.0f;
                endMoveTime = 0.0f;
                chargeAttack = false;
                jumpAttack = false;
                direction = Vector2.zero;
                whereToJump = Vector2.zero;
                firstMove = false;
            }
        }
        
        private void BringBabeDown(Transform emuTransform)
        {
            if (emuTransform.position.y > bringDownHeight)
            {
                emuTransform.GetComponent<Rigidbody2D>().gravityScale = bringDownGravityScale;
                emuTransform.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}