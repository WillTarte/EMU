using System;
using Player.States;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        #region Private Variables

        [Range(0, 10)] private int _hitPoints = 10;
        private BaseState _currentState;
        private InputHandler _inputHandler;
        private GameObject _nearestPickup;
        private bool _isHurt;
        private float _timer = 2;

        #endregion

        #region Interface Variables

        public EdgeCollider2D EdgeCollider { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRendererProp { get; private set; }
        public Animator Animator { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public TextMeshProUGUI WarningText;

        public GameObject NearestPickup
        {
            get => _nearestPickup;
            set => _nearestPickup = value;
        }

        public bool IsGrounded { get; private set; }
        public bool CanClimb { get; private set; }
        public bool IsFacingRight { get; private set; }

        public bool IsPressingLeft => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        public bool IsPressingRight => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        public bool IsPressingUp => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        public bool IsPressingDown => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        #endregion

        #region Public variables

        [Range(1, 10)] public float speed = 7.0f;
        [Range(100, 1000)] public float jumpForce = 500.0f;
        [Range(0, 500)] public int fallMultiplier = 10;


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
            _currentState?.Update(_inputHandler.HandleInput());
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
            }
        }

        public void LoseHitPoints(int value)
        {
            if (!_isHurt)
            {
                _isHurt = true;
                WarningText.enabled = true;
                _timer = 2;
                if (_hitPoints - value < 0) _hitPoints = 0;
                else _hitPoints -= value;

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

        private void OnCollisionEnter2D(Collision2D other)
        {

            if (other.gameObject.CompareTag("Ground"))
            {
                IsGrounded = true;
            }

            if (other.gameObject.CompareTag("Platform"))
            {
                if (Rigidbody.velocity.y >= 0.0F)
                {
                    IsGrounded = true;
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
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.parent != null && other.transform.parent.CompareTag("Weapon") &&
                other.CompareTag("WeaponOnGroundTrigger"))
            {
                if (NearestPickup != null)
                {
                    if (Vector2.Distance(transform.position, NearestPickup.transform.position) >
                        Vector2.Distance(transform.position, other.transform.parent.transform.position))
                    {
                        NearestPickup = other.transform.parent.gameObject;
                    }
                }
                else
                {
                    NearestPickup = other.transform.parent.gameObject;
                }
            }


            if (other.gameObject.CompareTag("Ladders"))
            {
                Debug.Log("Ladder hit");
                CanClimb = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Ladders"))
            {
                Debug.Log("Ladder left");
                CanClimb = false;
            }
            else if (other.transform.parent != null && other.transform.parent.CompareTag("Weapon") &&
                     other.CompareTag("WeaponOnGroundTrigger"))
            {
                NearestPickup = null;
            }
        }

        #endregion
    }
}