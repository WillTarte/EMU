using System.Collections.Generic;
using Player;
using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace HUD
{
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// Dictionary containing the inventory slots of the HUD inventory. Keys are slots, and values are the
        /// game objects displaying the HUD.
        /// </summary>
        private Dictionary<InventoryIndex, GameObject> _inventoryContainer;
        private InventoryIndex _selectedSlot;

        void Start()
        {
            _inventoryContainer = new Dictionary<InventoryIndex, GameObject>
            {
                {InventoryIndex.First, transform.Find("Weapon1").gameObject},
                {InventoryIndex.Second, transform.Find("Weapon2").gameObject},
                {InventoryIndex.Throwable, transform.Find("Throwable").gameObject}
            };
            
            _inventoryContainer[_selectedSlot].transform.Find("Selected").gameObject.SetActive(true);
        }

        //Outlines the weapon image in the HUD inventory that is currently used by the player.
        public void ChangeSelected(InventoryIndex newSelectedSlot)
        {
            _inventoryContainer[_selectedSlot].transform.Find("Selected").gameObject.SetActive(false);
            _selectedSlot = newSelectedSlot;
            _inventoryContainer[_selectedSlot].transform.Find("Selected").gameObject.SetActive(true);
        }

        public void RemoveFromInventory(InventoryIndex weaponSlot)
        {
            _inventoryContainer[weaponSlot].SendMessage("RemoveWeaponFromInventorySlot", SendMessageOptions.DontRequireReceiver);
        }

        public void AddToInventory(InventoryIndex slot, WeaponBehaviourScript weaponScript)
        {
            _inventoryContainer[slot].SendMessage("AddWeaponToInventorySlot", weaponScript, SendMessageOptions.DontRequireReceiver);
        }

        //Populate the HUD inventory with the inventory from InventoryManager
        public void UpdateFullInventory(Dictionary<InventoryIndex, WeaponBehaviourScript> inventory)
        {
            foreach (var pair in inventory)
            {
                if (pair.Value != null)  _inventoryContainer[pair.Key].SendMessage("AddWeaponToInventorySlot", pair.Value, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}