using System;
using Player.States;
using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        #region Private Variables
        
        private BaseState _currentState;
        private InputHandler _inputHandler;
        private GameObject _nearestPickup;

        #endregion

        #region Interface Variables
        
        public EdgeCollider2D EdgeCollider { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRendererProp { get; private set; }
        public Animator Animator { get; private set; }
        public InventoryManager InventoryManager { get; private set; }

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
        
        [Range(1, 10)]
        public float speed = 5.0F;
        [Range(100, 1000)]
        public float jumpForce = 400.0F;

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
            _currentState?.Update(_inputHandler.HandleInput());
            if (IsFacingRight
                ? !Vector2.right.Equals(InventoryManager.GetActiveWeapon()?.Direction)
                : !Vector2.left.Equals(InventoryManager.GetActiveWeapon()?.Direction))
            {
                if (InventoryManager.GetActiveWeapon() != null)
                {
                    InventoryManager.GetActiveWeapon().Direction = IsFacingRight ? Vector2.right : Vector2.left;
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

            Vector2[] edgeColliderPoints = {
                new Vector2(spriteCenter.x - spriteExtents.x, spriteCenter.y - spriteExtents.y),
                new Vector2(spriteCenter.x - spriteExtents.x, spriteCenter.y + spriteExtents.y),
                new Vector2(spriteCenter.x + spriteExtents.x, spriteCenter.y + spriteExtents.y),
                new Vector2(spriteCenter.x + spriteExtents.x, spriteCenter.y - spriteExtents.y),
                new Vector2(spriteCenter.x - spriteExtents.x, spriteCenter.y - spriteExtents.y),
            };
            EdgeCollider.points = edgeColliderPoints;
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

        #endregion
        
        #region Event Methods
        
        /// <summary>
        /// Event function called at the end of the Roll animation
        /// </summary>
        private void OnRollEnd()
        {
            Debug.Log("On Roll End");
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

            if (other.transform.parent != null && other.transform.parent.CompareTag("Weapon") && other.CompareTag("WeaponOnGroundTrigger"))
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
        }

        #endregion
    }
}