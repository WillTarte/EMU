
using ScriptableObjects.WeaponsSystem;
using UnityEngine;

namespace MonoBehaviours.WeaponsSystem
{
    /// <summary>
    /// Controls the behavior of a weapon when it is on the ground
    /// </summary>
    public class WeaponOnGroundBehaviour : MonoBehaviour
    {

        [SerializeField] private GameObject promptPrefab;
        private GameObject _triggerCollider;
        private WeaponData _weaponData;
        private BoxCollider2D _weaponCollider;
        private Rigidbody2D _weaponRigidBody;
        private SpriteRenderer _weaponSpriteRenderer;
        private bool _playerInside;
        
        /// <summary>
        /// Initializes some of this behaviour's params 
        /// </summary>
        /// <param name="weaponData"></param> The weapon's data
        public void Init(WeaponData weaponData)
        {
            _weaponData = weaponData;
        }

        private void Awake()
        {
            _weaponRigidBody = GetComponent<Rigidbody2D>();
            _weaponCollider = GetComponent<BoxCollider2D>();
            _weaponSpriteRenderer = GetComponent<SpriteRenderer>();

            _triggerCollider = new GameObject {layer = 10};
            _triggerCollider.transform.parent = transform;
            _triggerCollider.tag = "WeaponOnGroundTrigger";
            _triggerCollider.AddComponent<BoxCollider2D>();
            _triggerCollider.AddComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
            _triggerCollider.AddComponent<WeaponOnGroundTriggerScript>().Init(promptPrefab);
        
            _triggerCollider.SetActive(false);
        }

        private void OnEnable()
        {
            _weaponSpriteRenderer.sprite = _weaponData.OnGroundSprite;

            _weaponRigidBody.isKinematic = false;
            _weaponRigidBody.WakeUp();

            _weaponCollider.enabled = true;
            _weaponCollider.size = _weaponSpriteRenderer.sprite.bounds.size;
        
            _triggerCollider.SetActive(true);
        }

        private void OnDisable()
        {
            _weaponRigidBody.isKinematic = true;
            _weaponRigidBody.Sleep();

            _weaponCollider.enabled = false;
        
            _triggerCollider.SetActive(false);
        }
    }
}