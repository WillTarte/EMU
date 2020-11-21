using Interactables;
using ScriptableObjects.WeaponsSystem;
using UnityEngine;

namespace MonoBehaviours.WeaponsSystem
{
    /// <summary>
    /// Controls the behavior of a weapon when it is on the ground
    /// </summary>
    public class WeaponOnGroundBehaviour : MonoBehaviour
    {
        private const string GameobjectName = "WeaponOnGroundTriggerHitbox";
        
        [SerializeField] private GameObject promptPrefab;
        [SerializeField] private Vector2 promptHitboxSize;
        [SerializeField] private int promptTextSize;
        private GameObject _trigger;
        private WeaponData _weaponData;
        private BoxCollider2D _weaponCollider;
        private Rigidbody2D _weaponRigidBody;
        private SpriteRenderer _weaponSpriteRenderer;

        /// <summary>
        /// Initializes some of this behaviour's params 
        /// </summary>
        /// <param name="weaponData"></param> The weapon's data
        public void Init(WeaponData weaponData)
        {
            if (_weaponRigidBody == null)
            {
                _weaponRigidBody = GetComponent<Rigidbody2D>();
            }
            if (_weaponCollider == null)
            {
                _weaponCollider = GetComponent<BoxCollider2D>();
            }
            if (_weaponSpriteRenderer == null)
            {
                _weaponSpriteRenderer = GetComponent<SpriteRenderer>();
            }
            CreateTrigger();
            
            _weaponData = weaponData;
            _weaponSpriteRenderer.sprite = _weaponData.OnGroundSprite;
            _weaponRigidBody.isKinematic = false;
            _weaponRigidBody.WakeUp();

            _weaponCollider.enabled = true;
            _weaponCollider.size = _weaponSpriteRenderer.sprite.bounds.size;
        
            _trigger.SetActive(true);
        }

        private void CreateTrigger()
        {
            if (_trigger != null) return;
            _trigger = new GameObject {layer = LayerMask.NameToLayer("Trigger"), name = GameobjectName};
            _trigger.transform.parent = transform;
            _trigger.tag = "InteractTrigger";
            _trigger.AddComponent<BoxCollider2D>();
            _trigger.AddComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
            _trigger.AddComponent<InteractOnGroundTriggerScript>().Init(promptPrefab, promptHitboxSize, promptTextSize);
        
            _trigger.SetActive(false);
        }

        private void Awake()
        {
            _weaponRigidBody = GetComponent<Rigidbody2D>();
            _weaponCollider = GetComponent<BoxCollider2D>();
            _weaponSpriteRenderer = GetComponent<SpriteRenderer>();
            CreateTrigger();
        }

        private void OnEnable()
        {
            if (_weaponData == null) return;
            _weaponSpriteRenderer.sprite = _weaponData.OnGroundSprite;
            _weaponRigidBody.isKinematic = false;
            _weaponRigidBody.WakeUp();

            _weaponCollider.enabled = true;
            _weaponCollider.size = _weaponSpriteRenderer.sprite.bounds.size;
        
            _trigger.SetActive(true);
        }

        private void OnDisable()
        {
            _weaponRigidBody.isKinematic = true;
            _weaponRigidBody.Sleep();

            _weaponCollider.enabled = false;
        
            _trigger.SetActive(false);
        }
    }
}