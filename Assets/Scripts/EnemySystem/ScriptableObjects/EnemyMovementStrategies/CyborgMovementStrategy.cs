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

        private float _nextMoveTime;
        private float _teleportationTime;
        private float _enablePlayerMovementTime;

        private bool _isFlying;
        private bool _isTeleporting;
        private bool _isFalling;

        private bool _playerHasBeenPushed;

        private Vector2 _whereToFall;
        private Vector2 _startPosition;

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

                if (_nextMoveTime < Time.time)
                {
                    TeleportToPlayer(emuTransform, playerTransform);
                    return true;
                }
            }

            return false;
        }

        private void TeleportToPlayer(Transform emuTransform, Transform playerTransform)
        {
            if (!_isFlying)
            {
                emuTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = regularGravityScale;
                _startPosition = emuTransform.position;
                _isFlying = true;
            }

            if (emuTransform.position.y < _startPosition.y + flyingMaxY && !_isTeleporting)
            {
                emuTransform.position = new Vector2(emuTransform.position.x,
                    emuTransform.position.y + flyingStrength * Time.deltaTime);
            }
            else if (!_isTeleporting)
            {
                _teleportationTime = Time.time + teleportationDelay;
                _isTeleporting = true;
            }

            if (_isTeleporting && _teleportationTime < Time.time && !_isFalling)
            {
                _whereToFall = playerTransform.position;
                emuTransform.position = new Vector2(_whereToFall.x, emuTransform.position.y);
                emuTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = bringDownGravityScale;
                _isFalling = true;
            }

            if (_isFalling && emuTransform.position.y <= _startPosition.y)
            {
                _nextMoveTime = Time.time + timeOnFloor;
                _isFalling = false;
                _isFlying = false;
                _isTeleporting = false;
            }
        }

        private void PushPlayerAway(Transform emuTransform, Transform playerTransform)
        {
            if (_nextMoveTime < Time.time)
            {
                emuTransform.GetChild(1).gameObject.SetActive(true);
                if (!_playerHasBeenPushed && PlayerInPushRadius(emuTransform, playerTransform))
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
                    _playerHasBeenPushed = true;
                    _enablePlayerMovementTime = Time.time + timePlayerPushed;
                    playerTransform.GetComponent<Controller>().setPlayerInputsEnabled(false);
                }
            }
            else
            {
                emuTransform.GetChild(1).gameObject.SetActive(false);
            }

            if (_enablePlayerMovementTime < Time.time && _playerHasBeenPushed)
            {
                playerTransform.GetComponent<Controller>().setPlayerInputsEnabled(true);
                playerTransform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _playerHasBeenPushed = false;
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
                _isFalling = false;
                _isFlying = false;
                _isTeleporting = false;
                _playerHasBeenPushed = false;
                _nextMoveTime = 0;
                _teleportationTime = 0;
                _enablePlayerMovementTime = 0;
                _whereToFall = Vector2.zero;
                _startPosition = Vector2.zero;
            }
        }
    }
}