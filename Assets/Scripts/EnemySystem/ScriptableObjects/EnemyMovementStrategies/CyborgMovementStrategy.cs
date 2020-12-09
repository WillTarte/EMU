using EnemySystem.Monobehaviours;
using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyMovementStrategies
{
    [CreateAssetMenu(fileName = "CyborgMovementStrategy",
        menuName = "ScriptableObjects/EnemyMovement/CyborgMovement", order = 3)]
    public class CyborgMovementStrategy : EnemyMovementStrategy
    {
        #region Interface Variables
        
        //Atomic parameters of cyborg emu
        [SerializeField] private float platformStartX;
        [SerializeField] private float platformEndX;
        [SerializeField] private float flyingMaxY;
        [SerializeField] private float flyingStrength;
        [SerializeField] private float regularGravityScale;
        [SerializeField] private float bringDownGravityScale;
        [SerializeField] private float pushStrength;
        [SerializeField] private float pushRadius;
        [SerializeField] private float teleportationDelay;
        [SerializeField] private float timeOnFloor;

        
        #endregion

        #region Private Variables
        
        private float nextMoveTime;
        private float teleportationTime;

        private bool isFlying;
        private bool isTeleporting;
        private bool isFalling;

        private bool playerHasBeenPushed;

        private Vector2 whereToFall;
        private Vector2 startPosition;

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
                PushPlayerAway(emuTransform, playerTransform);
                
                if (nextMoveTime < Time.time)
                {
                    TeleportToPlayer(emuTransform, playerTransform);
                    return true;
                }
                
            }

            return false;
        }
        
        private void TeleportToPlayer(Transform emuTransform, Transform playerTransform)
        {
            if (!isFlying)
            {
                emuTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = regularGravityScale;
                startPosition = emuTransform.position;
                isFlying = true;
            }

            if (emuTransform.position.y < startPosition.y + flyingMaxY && !isTeleporting)
            {
                emuTransform.position = new Vector2(emuTransform.position.x, emuTransform.position.y + flyingStrength * Time.deltaTime);
            }
            else if (!isTeleporting)
            {
                teleportationTime = Time.time + teleportationDelay;
                isTeleporting = true;
            }

            if (isTeleporting && teleportationTime < Time.time && !isFalling)
            {
                whereToFall = playerTransform.position;
                emuTransform.position = new Vector2(whereToFall.x, emuTransform.position.y);
                emuTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = bringDownGravityScale;
                isFalling = true;
            }

            if (isFalling && emuTransform.position.y <= startPosition.y)
            {
                nextMoveTime = Time.time + timeOnFloor;
                isFalling = false;
                isFlying = false;
                isTeleporting = false;
            }
        }

        private void PushPlayerAway(Transform emuTransform, Transform playerTransform)
        {
            if (nextMoveTime < Time.time)
            {
                emuTransform.GetChild(1).gameObject.SetActive(true);
                if (!playerHasBeenPushed && PlayerInPushRadius(emuTransform, playerTransform))
                {
                    Vector2 direction = Vector2.MoveTowards(playerTransform.position, emuTransform.position, pushStrength);
                    playerTransform.GetComponent<Rigidbody2D>().AddForce(-direction);
                    playerHasBeenPushed = true;
                }
            }
            else
            {
                emuTransform.GetChild(1).gameObject.SetActive(false);
                playerHasBeenPushed = false;
            }
        }

        private bool PlayerInPushRadius(Transform emuTransform, Transform playerTransform)
        {
            return Vector2.Distance(emuTransform.position, playerTransform.position) < 2;
        }
        
        private void ResetValuesAtStart()
        {
            if (Time.time < 0.1f)
            {
                isFalling = false;
                isFlying = false;
                isTeleporting = false;
                playerHasBeenPushed = false;
                nextMoveTime = 0;
                teleportationTime = 0;
                whereToFall = Vector2.zero;
                startPosition = Vector2.zero;
            }
        }

    }
}