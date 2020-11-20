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
        [SerializeField] private int amount;

        private Rigidbody2D _rigidbody;
        private GameObject _trigger;
        private bool _forceApplied;

        public void Init(int amt)
        {
            amount = amt;
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

        private void Update()
        {
            if (amount == 0)
            {
                Destroy(gameObject);
            }
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
            if (playerController.InventoryManager.WeaponSlots[InventoryIndex.First] == null &&
                playerController.InventoryManager.WeaponSlots[InventoryIndex.Second] == null)
            {
                return;
            }

            if (playerController.InventoryManager.GetActiveWeapon() != null)
            {
                if (playerController.InventoryManager.GetActiveWeapon().CurrentMagazineAmmunition !=
                    playerController.InventoryManager.GetActiveWeapon().WeaponData.MagazineCapacity && amount > 0)
                {
                    var amountToReload = Math.Min(playerController.InventoryManager.GetActiveWeapon().WeaponData.MagazineCapacity -
                                                  playerController.InventoryManager.GetActiveWeapon().CurrentMagazineAmmunition, amount);
                    amount -= amountToReload;
                
                    playerController.InventoryManager.GetActiveWeapon().CurrentMagazineAmmunition += amountToReload;
                }

                if (playerController.InventoryManager.GetActiveWeapon().CurrentTotalAmmunition !=
                    playerController.InventoryManager.GetActiveWeapon().WeaponData.MaxAmmunitionCount && amount > 0)
                {
                    var amountToReload = Math.Min(playerController.InventoryManager.GetActiveWeapon().WeaponData.MaxAmmunitionCount -
                                                  playerController.InventoryManager.GetActiveWeapon().CurrentTotalAmmunition, amount);
                    amount -= amountToReload;
                
                    playerController.InventoryManager.GetActiveWeapon().CurrentTotalAmmunition += amountToReload;
                }
            }
            
            var otherWeaponIndex = playerController.InventoryManager.CurrentActiveWeaponSlot == InventoryIndex.First
                ? InventoryIndex.Second
                : InventoryIndex.First;
            
            if (playerController.InventoryManager.WeaponSlots[otherWeaponIndex] == null)
            {
                return;
            }
            
            if (playerController.InventoryManager.WeaponSlots[otherWeaponIndex].CurrentMagazineAmmunition !=
                playerController.InventoryManager.WeaponSlots[otherWeaponIndex].WeaponData.MagazineCapacity && amount > 0)
            {
                var amountToReload = Math.Min(playerController.InventoryManager.WeaponSlots[otherWeaponIndex].WeaponData.MagazineCapacity -
                                              playerController.InventoryManager.WeaponSlots[otherWeaponIndex].CurrentMagazineAmmunition, amount);
                amount -= amountToReload;
                
                playerController.InventoryManager.WeaponSlots[otherWeaponIndex].CurrentMagazineAmmunition += amountToReload;
            }

            if (playerController.InventoryManager.WeaponSlots[otherWeaponIndex].CurrentTotalAmmunition !=
                playerController.InventoryManager.WeaponSlots[otherWeaponIndex].WeaponData.MaxAmmunitionCount && amount > 0)
            {
                var amountToReload = Math.Min(playerController.InventoryManager.WeaponSlots[otherWeaponIndex].WeaponData.MaxAmmunitionCount -
                                              playerController.InventoryManager.WeaponSlots[otherWeaponIndex].CurrentTotalAmmunition, amount);
                amount -= amountToReload;
                
                playerController.InventoryManager.WeaponSlots[otherWeaponIndex].CurrentTotalAmmunition += amountToReload;
            }

            if (amount <= 0)
            {
                Destroy(gameObject); 
            }
        }
    }
}
