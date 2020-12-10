using System;
using EnemySystem.Monobehaviours;
using Player;
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
        [SerializeField] private float timePlayerPushed;

        #endregion

        #region Private Variables
        
        //Variables used to coordinates CyborgEmu movements
        private float nextMoveTime;
        private float teleportationTime;
        private float enablePlayerMovementTime;

        //Variables used to define CyborgEmu movements
        private bool isFlying;
        private bool isTeleporting;
        private bool isFalling;

        //Variables used for CyborgEmu movements
        private Vector2 whereToFall;
        private Vector2 startPosition;
        
        //Avoid pushing the player multiple times at once
        private bool playerHasBeenPushed;
        private static readonly int Idle = Animator.StringToHash("Idle");

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
                //Push the player away if too close of the boss
                PushPlayerAway(emuTransform, playerTransform);

                if (nextMoveTime < Time.time)
                {
                    //Make the boss change position on the platform
                    return TeleportToPlayer(emuTransform, playerTransform);
                }
            }

            return false;
        }
        
        private bool TeleportToPlayer(Transform emuTransform, Transform playerTransform)
        {
            //Prepare the boss to start flying
            if (!isFlying)
            {
                emuTransform.gameObject.GetComponent<Animator>().SetBool(Idle, true);
                emuTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = regularGravityScale;
                startPosition = emuTransform.position;
                isFlying = true;
            }

            //Make the boss rise up
            if (emuTransform.position.y < startPosition.y + flyingMaxY && !isTeleporting)
            {
                emuTransform.position = new Vector2(emuTransform.position.x,
                    emuTransform.position.y + flyingStrength * Time.deltaTime);
            }
            //Once the boss is high enough, set the time the teleport will occur
            else if (!isTeleporting)
            {
                teleportationTime = Time.time + teleportationDelay;
                isTeleporting = true;
            }

            //Teleport the emu on top of the player and let it fall on him
            if (isTeleporting && teleportationTime < Time.time && !isFalling)
            {
                whereToFall = playerTransform.position;
                emuTransform.position = new Vector2(whereToFall.x, emuTransform.position.y);
                emuTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = bringDownGravityScale;
                isFalling = true;
            }

            //Once the movement is over, reset the bools and set the time of the next movement
            if (isFalling && emuTransform.position.y <= startPosition.y)
            {
                emuTransform.gameObject.GetComponent<Animator>().SetBool(Idle, false);
                nextMoveTime = Time.time + timeOnFloor;
                isFalling = false;
                isFlying = false;
                isTeleporting = false;
            }

            return true;
        }

        private void PushPlayerAway(Transform emuTransform, Transform playerTransform)
        {
            if (nextMoveTime < Time.time)
            {
                //Start the shield animation
                emuTransform.GetChild(2).gameObject.SetActive(true);
                //Push the player away if he is in range
                if (!playerHasBeenPushed && PlayerInPushRadius(emuTransform, playerTransform))
                {
                    Vector2 direction = emuTransform.position - playerTransform.position;
                    if (direction.x != 0)
                    {
                        direction = new Vector2(Math.Sign(direction.x) * -pushStrength, 0);
                    }
                    else
                    {
                        if (CloserToRightEdge(emuTransform))
                        {
                            direction = new Vector2(-pushStrength, 0);
                        }
                        else
                        {
                            direction = new Vector2(pushStrength, 0);
                        }
                    }

                    playerTransform.GetComponent<Rigidbody2D>().AddForce(direction);
                    playerHasBeenPushed = true;
                    //The player inputs are disabled while being pushed away
                    enablePlayerMovementTime = Time.time + timePlayerPushed;
                    playerTransform.GetComponent<Controller>().setPlayerInputsEnabled(false);
                }
            }
            else
            {
                //Remove shield animation, once the boss is done moving
                emuTransform.GetChild(2).gameObject.SetActive(false);
            }

            if (enablePlayerMovementTime < Time.time && playerHasBeenPushed)
            {
                //Enable player movements and stop the push vector
                playerTransform.GetComponent<Controller>().setPlayerInputsEnabled(true);
                playerTransform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerHasBeenPushed = false;
            }
        }

        private bool PlayerInPushRadius(Transform emuTransform, Transform playerTransform)
        {
            return Vector2.Distance(emuTransform.position, playerTransform.position) < pushRadius;
        }

        private bool CloserToRightEdge(Transform emuTransform)
        {
            return emuTransform.position.x - platformStartX <= platformEndX - emuTransform.position.x;
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
                enablePlayerMovementTime = 0;
                whereToFall = Vector2.zero;
                startPosition = Vector2.zero;
            }
        }
    }
}