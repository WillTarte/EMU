
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Player.Commands;
using Player.States;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        private readonly float _maxFallthroughTime = 0.5f;

        #region Private Variables

        [Range(0, 10)] private int _hitPoints = 10;
        private BaseState _currentState;
        private InputHandler _inputHandler;
        private bool _isHurt;
        private float _timer = 2;
        private float _fallthroughTimer = 0.0f;
        private List<GameObject> _nearestInteractables = new List<GameObject>(1);

        #endregion

        #region Interface Variables

        public GameObject bloodEffect;
        public bool IsFullHp => _hitPoints == 10;
        
        public EdgeCollider2D EdgeCollider { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRendererProp { get; private set; }
        public Animator Animator { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public BaseState CurrentState => _currentState;

        public TextMeshProUGUI WarningText;
        
        [CanBeNull] public GameObject NearestInteractable => _nearestInteractables.Count > 0 ? _nearestInteractables[0] : null;
        public void RemoveInteractable(GameObject interactable) => _nearestInteractables.Remove(interactable);

        public bool IsGrounded { get; private set; }
        public bool IsOnPlatform { get; private set; }
        public bool CanClimb { get; private set; }
        public bool IsFacingRight { get; private set; }

        public bool CanFallthrough { get; set; }

        public bool IsPressingLeft => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        public bool IsPressingRight => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        public bool IsPressingUp => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        public bool IsPressingDown => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        #endregion

        #region Public variables

        [Range(1, 10)] public float speed = 7.0f;
        [Range(100, 1000)] public float jumpForce = 500.0f;
        [Range(0, 500)] public int fallMultiplier = 10;

        public delegate void OnStateChange(BaseState newState);
        public event OnStateChange OnStateChanged;

        public delegate void OnCommandInput(Command command);
        public event OnCommandInput OnCommandInputted;

        public delegate void OnDamageTaken();
        public event OnDamageTaken OnDamageTakenEvent;

        /// <summary>
        /// Events listened by the HUD to update the HUD health bar. Delegates allows to pass variable using events.
        /// </summary>
        public delegate void UpdateHUDHealthBarHandler(int hitPoints);

        public event UpdateHUDHealthBarHandler UpdateHealthBarHUD;
        public event UpdateHUDHealthBarHandler ResetHealthBarHUD;

        #endregion

        #region Private Methods

        // Start is called before the first frame update
        private void Start()
        {
            _inputHandler = new InputHandler();

            EdgeCollider = GetComponent<EdgeCollider2D>();
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRendererProp = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
            InventoryManager = GetComponent<InventoryManager>();

            WarningText.enabled = false;

            IsGrounded = false;
            IsOnPlatform = false;
            IsFacingRight = true;

            SetEdgeColliderPoints();

            ChangeState(new IdleState());
        }

        private void FixedUpdate()
        {
            SetEdgeColliderPoints();
        }

        // Update is called once per frame
        private void Update()
        {
            IsHurt();
            var input = _inputHandler.HandleInput();
            if (input != null)
            {
                OnCommandInputted?.Invoke(input);
            }
            _currentState?.Update(input);
            if (IsFacingRight
                ? !Vector2.right.Equals(InventoryManager.GetActiveWeapon()?.Direction)
                : !Vector2.left.Equals(InventoryManager.GetActiveWeapon()?.Direction))
            {
                if (InventoryManager.GetActiveWeapon() != null)
                {
                    InventoryManager.GetActiveWeapon().Direction = IsFacingRight ? Vector2.right : Vector2.left;
                }

                if (InventoryManager.GetThrowableWeapon() != null)
                {
                    InventoryManager.GetThrowableWeapon().Direction = IsFacingRight ? Vector2.right : Vector2.left;
                }
            }

            if (CanFallthrough)
            {
                _fallthroughTimer += Time.deltaTime;
            }

            if (_fallthroughTimer >= _maxFallthroughTime)
            {
                _fallthroughTimer = 0.0f;

                CanFallthrough = false;
            }

            _nearestInteractables.Sort(delegate(GameObject o, GameObject o1)
            {
                var distanceToPlayer0 = Vector2.Distance(transform.position, o.transform.position);
                var distanceToPlayer1 = Vector2.Distance(transform.position, o1.transform.position);
                if (Math.Abs(distanceToPlayer0 - distanceToPlayer1) < 0.1) return 0;
                if (distanceToPlayer0 < distanceToPlayer1) return -1;
                return 1;
            });
        }

        private void FlipSprite()
        {
            IsFacingRight = !IsFacingRight;

            var scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }

        private void SetEdgeColliderPoints()
        {
            var sprite = SpriteRendererProp.sprite;
            Vector2 spriteCenter = sprite.bounds.center;
            Vector2 spriteExtents = sprite.bounds.extents;
            
            if (spriteExtents.x > 0.25f) {
                Vector2[] edgeColliderPoints =
                    {
                        new Vector2(spriteCenter.x - 0.25f, spriteCenter.y - spriteExtents.y),
                        new Vector2(spriteCenter.x - 0.25f, spriteCenter.y + spriteExtents.y),
                        new Vector2(spriteCenter.x + 0.25f, spriteCenter.y + spriteExtents.y),
                        new Vector2(spriteCenter.x + 0.25f, spriteCenter.y - spriteExtents.y),
                        new Vector2(spriteCenter.x - 0.25f, spriteCenter.y - spriteExtents.y),
                    };
                EdgeCollider.points = edgeColliderPoints;
            }
            else
            {
                Vector2[] edgeColliderPoints = 
                {
                    new Vector2(spriteCenter.x - spriteExtents.x, spriteCenter.y - spriteExtents.y),
                    new Vector2(spriteCenter.x - spriteExtents.x, spriteCenter.y + spriteExtents.y),
                    new Vector2(spriteCenter.x + spriteExtents.x, spriteCenter.y + spriteExtents.y),
                    new Vector2(spriteCenter.x + spriteExtents.x, spriteCenter.y - spriteExtents.y),
                    new Vector2(spriteCenter.x - spriteExtents.x, spriteCenter.y - spriteExtents.y),
                };
                EdgeCollider.points = edgeColliderPoints;
            }
        }

        private void IsHurt()
        {
            _timer -= Time.deltaTime;
            if (_timer < 0 && _isHurt)
            {
                _timer = 2;
                _isHurt = false;
                WarningText.enabled = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the direction of the texture based on keyboard input
        /// </summary>
        public void UpdateTextureDirection()
        {
            if (IsPressingRight)
            {
                if (!IsFacingRight)
                {
                    FlipSprite();
                }
            }
            else if (IsPressingLeft)
            {
                if (IsFacingRight)
                {
                    FlipSprite();
                }
            }
        }

        public void MoveX(float fixedSpeed)
        {
            float moveBy = fixedSpeed * speed;
            Rigidbody.velocity = new Vector2(moveBy, Rigidbody.velocity.y);
        }

        public void MoveY(float fixedSpeed)
        {
            float moveBy = fixedSpeed * speed;
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, moveBy);
        }

        public void ResetSpriteFlip()
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);

            if (!IsFacingRight)
            {
                scale.x *= -1;
            }

            transform.localScale = scale;
        }

        public void CheckForRoll()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (IsGrounded)
                {
                    ChangeState(new RollState {XAxisRaw = Input.GetAxisRaw("Horizontal")});
                }
            }
        }

        /// <summary>
        /// Method used to change state
        /// </summary>
        /// <param name="newState">New state.</param>
        public void ChangeState(BaseState newState)
        {
            _currentState?.Destroy();

            _currentState = newState;

            if (_currentState != null)
            {
                _currentState.Controller = this;
                _currentState.Start();
                OnStateChanged?.Invoke(newState);
            }
        }

        public void LoseHitPoints(int value)
        {
            if (!_isHurt && !(_currentState is RollState))
            {
                _isHurt = true;
                WarningText.enabled = true;
                _timer = 2;
                if (_hitPoints - value < 0) _hitPoints = 0;
                else _hitPoints -= value;

                OnDamageTakenEvent?.Invoke();
                if (_hitPoints <= 0)
                {
                    StartCoroutine(GameOver());
                }

                UpdateHealthBarHUD?.Invoke(_hitPoints);
                
            }
        }

        public void RestoreHitPoints(int value)
        {
            if (_hitPoints + value > 10) _hitPoints = 10;
            else _hitPoints += value;

            UpdateHealthBarHUD?.Invoke(_hitPoints);
        }

        public void ResetHitPoints()
        {
            _hitPoints = 10;
            ResetHealthBarHUD?.Invoke(_hitPoints);
        }

        private IEnumerator GameOver()
        {
            SpriteRendererProp.sprite = null;
            Animator.enabled = false;
            enabled = false;
            var blood = Instantiate(bloodEffect, transform.position, transform.rotation);
            GetComponent<SoundController>()?.PlayGameOverAudio();
            yield return new WaitWhile(() => GetComponent<AudioSource>().isPlaying);
            Destroy(blood);
            Destroy(gameObject);
            SceneManager.LoadScene(3);
        }

        #endregion

        #region Event Methods

        /// <summary>
        /// Event function called at the end of the Roll animation
        /// </summary>
        private void OnRollEnd()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                IsFacingRight = true;
                ChangeState(new RunState());
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                IsFacingRight = false;
                ChangeState(new RunState());
            }
            else
            {
                ChangeState(new IdleState());
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {

            if (other.gameObject.CompareTag("Ground"))
            {
                //Debug.Log("On Ground | Bridge");

                IsGrounded = true;
            }

            if (other.gameObject.CompareTag("Platform"))
            {
                if (Rigidbody.velocity.y >= 0.0F)
                {
                    IsGrounded = true;
                    IsOnPlatform = true;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                IsGrounded = false;
            }

            if (other.gameObject.CompareTag("Platform"))
            {
                IsGrounded = false;
                IsOnPlatform = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteractTrigger"))
            {
                if (other.transform.parent.CompareTag("AmmoPickup") ||
                    other.transform.parent.CompareTag("HealthPickup")) return;
                _nearestInteractables.Add(other.transform.parent.gameObject);
            }
            else if (other.gameObject.CompareTag("Ladders"))
            {
                CanClimb = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Ladders"))
            {
                CanClimb = false;
            }
            else if (other.CompareTag("InteractTrigger"))
            {
                _nearestInteractables.Remove(other.transform.parent.gameObject);
            }
        }
        #endregion
    }
}