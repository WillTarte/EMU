using System;
using UnityEngine;
using WeaponsSystem.ScriptableObjects;

namespace WeaponsSystem.MonoBehaviours
{
    /// <summary>
    /// Controls the behavior of a weapon.  
    /// A weapon's behaviour and look is defined by a WeaponData (scriptable object instance) and a state
    /// </summary>
    public class WeaponBehaviourScript : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private WeaponState weaponState;
        [SerializeField] private int currentMagazineAmmunition;
        [SerializeField] private int currentTotalAmmunition;
        private WeaponOnGroundBehaviour _weaponOnGroundBehaviour;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private Vector2 _direction = Vector2.right;

        /// <summary>
        /// Returns the world space position of where the weapon's muzzle is
        /// </summary>
        public Vector2 WeaponShootLocation
        {
            get
            {
                if (gameObject.transform.parent != null)
                {
                    var parent = gameObject.transform.parent.gameObject;
                    var parentSpriteRender = parent.GetComponent<SpriteRenderer>();
                    var sprite = parentSpriteRender.sprite;
                    if (gameObject.transform.parent.CompareTag("Player"))
                    {
                        return (Vector2) parent.transform.position +
                               new Vector2(sprite.bounds.extents.x, 0);
                    }
                    return parent.transform.position;
                }
                return (Vector2) transform.position + (Direction * new Vector2(_spriteRenderer.sprite.bounds.extents.x, _spriteRenderer.sprite.bounds.extents.y));
            }
        }

        public WeaponData WeaponData
        {
            get => weaponData;
            set
            {

                weaponData = value.Copy();

                _weaponOnGroundBehaviour.Init(weaponData);
                if (currentTotalAmmunition > weaponData.MaxAmmunitionCount)
                {
                    currentTotalAmmunition = weaponData.MaxAmmunitionCount;
                }

                if (currentMagazineAmmunition > weaponData.MagazineCapacity)
                {
                    currentMagazineAmmunition = weaponData.MagazineCapacity;
                }
            }
        }

        /// <summary>
        /// Property for the weapon's direction
        /// </summary>
        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public WeaponState WeaponStateProp
        {
            get => weaponState;
            set => weaponState = value;
        }

        public int CurrentMagazineAmmunition
        {
            get => currentMagazineAmmunition;
            set => currentMagazineAmmunition = value > weaponData.MagazineCapacity ? weaponData.MagazineCapacity : value;
        }

        public int CurrentTotalAmmunition
        {
            get => currentTotalAmmunition;
            set => currentTotalAmmunition = value > weaponData.MaxAmmunitionCount ? weaponData.MaxAmmunitionCount : value;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.sleepMode = RigidbodySleepMode2D.StartAsleep;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _weaponOnGroundBehaviour = GetComponent<WeaponOnGroundBehaviour>();

            if (weaponData != null)
            {
                // Didn't find much info, but hopefully this does a deep copy of everything
                weaponData = weaponData.Copy();
                
                _weaponOnGroundBehaviour.Init(weaponData);
                if (currentTotalAmmunition > weaponData.MaxAmmunitionCount)
                {
                    currentTotalAmmunition = weaponData.MaxAmmunitionCount;
                }

                if (currentMagazineAmmunition > weaponData.MagazineCapacity)
                {
                    currentMagazineAmmunition = weaponData.MagazineCapacity;
                }
            }
        }

        private void Update()
        {
            switch (weaponState)
            {
                case WeaponState.OnGround:
                    _weaponOnGroundBehaviour.enabled = true;
                    _rigidbody.WakeUp();
                    break;
                case WeaponState.InInventory:
                    _weaponOnGroundBehaviour.enabled = false;
                    _spriteRenderer.sprite = null;
                    _rigidbody.Sleep();
                    break;
                case WeaponState.Inactive:
                    _weaponOnGroundBehaviour.enabled = false;
                    _spriteRenderer.sprite = null;
                    break;
                case WeaponState.Active:
                    _weaponOnGroundBehaviour.enabled = false;
                    _spriteRenderer.sprite = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void Shoot()
        {
            if (weaponState != WeaponState.Active)
            {
                Debug.Log("Tried to shoot with " + gameObject.name + " but was not active");
                return;
            }
            weaponData.ShootStrategy.Shoot(this);
        }

        public void Reload()
        {
            if (weaponState != WeaponState.Active)
            {
                Debug.Log("Tried to reload with " + gameObject.name + " but was not active");
                return;
            }
            weaponData.ShootStrategy.Reload(this);
        }
    }
    
    public enum WeaponState
    {
        OnGround,
        InInventory,
        Inactive,
        Active
    }
}
