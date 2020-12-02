using System;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactables
{
    public class AmmoPickupBehaviour : MonoBehaviour, IAutoPickup
    {
        private const string GameObjectName = "AmmoPickupTrigger";

        [SerializeField] private Vector2 pickupTriggerSize;
        [SerializeField] private int percentageToReplenish;

        private Rigidbody2D _rigidbody;
        private GameObject _trigger;
        private bool _forceApplied;

        public void Init(int percentage)
        {
            percentageToReplenish = percentage;
        }

        private void Awake()
        {
            _trigger = new GameObject {layer = LayerMask.NameToLayer("Trigger"), name = GameObjectName};
            _trigger.transform.parent = transform;
            _trigger.tag = "InteractTrigger";
            _trigger.AddComponent<BoxCollider2D>();
            _trigger.AddComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
            _trigger.AddComponent<AutoPickupTriggerScript>().Init(pickupTriggerSize);
            
            _trigger.SetActive(true);

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_forceApplied)
            {
                _rigidbody.AddForce(new Vector2(Random.Range(0, 5), Random.Range(0, 5)), ForceMode2D.Impulse);
                _forceApplied = true;
            }
        }

        public void Pickup(Controller playerController)
        {
            if (_rigidbody.velocity.y != 0) return;
            
            if (playerController.InventoryManager.WeaponSlots[InventoryIndex.First] == null &&
                playerController.InventoryManager.WeaponSlots[InventoryIndex.Second] == null)
            {
            }
            else
            {
                var activeWeapon = playerController.InventoryManager.WeaponSlots[
                    playerController.InventoryManager.CurrentActiveWeaponSlot];
                var otherWeaponIndex = playerController.InventoryManager.CurrentActiveWeaponSlot == InventoryIndex.First
                    ? InventoryIndex.Second
                    : InventoryIndex.First;
                var otherWeapon = playerController.InventoryManager.WeaponSlots[otherWeaponIndex];

                if (activeWeapon != null && activeWeapon.CurrentTotalAmmunition < activeWeapon.WeaponData.MaxAmmunitionCount)
                {
                    var newAmmoCount = Math.Min(activeWeapon.WeaponData.MaxAmmunitionCount * (percentageToReplenish / 100.0f) + activeWeapon.CurrentTotalAmmunition, activeWeapon.WeaponData.MaxAmmunitionCount);
                    activeWeapon.CurrentTotalAmmunition = (int) newAmmoCount;
                    
                    Destroy(gameObject);
                } 
                else if (otherWeapon != null &&
                         otherWeapon.CurrentTotalAmmunition < otherWeapon.WeaponData.MaxAmmunitionCount)
                {
                    var newAmmoCount = Math.Min(activeWeapon.WeaponData.MaxAmmunitionCount * (percentageToReplenish / 100.0f) + activeWeapon.CurrentTotalAmmunition, otherWeapon.WeaponData.MaxAmmunitionCount);
                    otherWeapon.CurrentTotalAmmunition = (int) newAmmoCount;
                    Destroy(gameObject);
                }
            }
        }
    }
}
