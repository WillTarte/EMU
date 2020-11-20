using System;
using System.Collections.Generic;
using MonoBehaviours;
using MonoBehaviours.WeaponsSystem;
using ScriptableObjects.WeaponsSystem;
using UnityEditor;
using UnityEngine;

namespace Interactables
{
    public class ChestBehaviourScript : MonoBehaviour
    {
        private const string GameobjectName = "ChestInteractTrigger";

        [SerializeField] private HealthPickupParams healthPickupParams;
        [SerializeField] private AmmoPickupParams ammoPickupParams;
        [SerializeField] private List<WeaponPickupParams> weaponPickupList;

        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private GameObject ammoPickupPrefab;
        [SerializeField] private GameObject healthPickupPrefab;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private GameObject promptPrefab;
        [SerializeField] private Vector2 promptHitboxSize;
        [SerializeField] private int promptTextSize;

        private GameObject _triggerCollider;
        private SpriteRenderer _spriteRenderer;
        
        private bool _isOpen;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _isOpen ? openSprite : closedSprite;
            
            _triggerCollider = new GameObject {layer = LayerMask.NameToLayer("Trigger"), name = GameobjectName};
            _triggerCollider.transform.parent = transform;
            _triggerCollider.tag = "InteractTrigger";
            _triggerCollider.AddComponent<BoxCollider2D>();
            _triggerCollider.AddComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
            _triggerCollider.AddComponent<InteractOnGroundTriggerScript>().Init(promptPrefab, promptHitboxSize, promptTextSize);
        
            _triggerCollider.SetActive(true);
        }

        public void Interact()
        {

            if (_isOpen)
            {
                Debug.Log("Tried Interacting with an already open chest");
                _spriteRenderer.sprite = openSprite;
                _triggerCollider.SetActive(false);
                enabled = false;
            }
            else
            {
                for (var i = 0; i < healthPickupParams.numberOfPickups; i++)
                {
                    var pickup = Instantiate(healthPickupPrefab, (Vector2) transform.position + new Vector2(0f, 0.5f),
                        Quaternion.identity);
                    pickup.GetComponent<HealthPickupBehaviour>().Init(healthPickupParams.healthAmount);
                }

                for (var i = 0; i < ammoPickupParams.numberOfPickups; i++)
                {
                    var pickup = Instantiate(ammoPickupPrefab, (Vector2) transform.position + new Vector2(0f, 0.5f),
                        Quaternion.identity);
                    pickup.GetComponent<AmmoPickupBehaviour>().Init(ammoPickupParams.ammoAmount);
                }

                foreach (var weaponParam in weaponPickupList)
                {
                    var droppedWeapon = GameObject.Instantiate(weaponPrefab,
                        (Vector2) transform.position + new Vector2(0f, 0.5f), Quaternion.identity);

                    var droppedWeaponBehaviour = droppedWeapon.GetComponent<WeaponBehaviourScript>();
                    droppedWeaponBehaviour.WeaponData = WeaponDataDictionary.weaponDataDictionary[weaponParam.weaponName];
                    droppedWeaponBehaviour.CurrentMagazineAmmunition = weaponParam.magazineAmmunition;
                    droppedWeaponBehaviour.CurrentTotalAmmunition = weaponParam.totalAmmunition;
                    droppedWeaponBehaviour.WeaponStateProp = WeaponState.OnGround;
                }
                
                _isOpen = true;
                _spriteRenderer.sprite = openSprite;
                _triggerCollider.SetActive(false);
                enabled = false;
            }
        }
    }

    // This is fucking hacky
    internal static class WeaponDataDictionary
    {
        public static Dictionary<WeaponName, WeaponData> weaponDataDictionary;

        static WeaponDataDictionary()
        {
            weaponDataDictionary = new Dictionary<WeaponName, WeaponData>()
            {
                {WeaponName.AssaultRifle, (WeaponData) AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Instances/WeaponsSystem/WeaponData/AssaultRifleData.asset", typeof(WeaponData))},
                {WeaponName.Sniper, (WeaponData) AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Instances/WeaponsSystem/WeaponData/SniperData.asset", typeof(WeaponData))},
                {WeaponName.Shotgun, (WeaponData) AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Instances/WeaponsSystem/WeaponData/ShotgunData.asset", typeof(WeaponData))},
                {WeaponName.Knife, (WeaponData) AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Instances/WeaponsSystem/WeaponData/KnifeData.asset", typeof(WeaponData))},
                {WeaponName.Grenade, (WeaponData) AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Instances/WeaponsSystem/WeaponData/GrenadeData.asset", typeof(WeaponData))}
            };
        }
    }

    [Serializable]
    internal struct HealthPickupParams
    {
        public int numberOfPickups;
        public int healthAmount;
    }

    [Serializable]
    internal struct AmmoPickupParams
    {
        public int numberOfPickups;
        public int ammoAmount;
    }

    [Serializable]
    internal struct WeaponPickupParams
    {
        public WeaponName weaponName;
        public int magazineAmmunition;
        public int totalAmmunition;
    }
}