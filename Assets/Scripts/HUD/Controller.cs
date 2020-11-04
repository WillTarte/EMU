using Player;
using UnityEngine;

namespace HUD
{
    public class Controller : MonoBehaviour
    {
        public HealthBar HealthBar;
        public Inventory Inventory;

        //Used to access hitPoints of player
        private Player.Controller _playerController;
        //Used to access the weapons owned by player
        private InventoryManager _playerInventoryManager;
        
        void Start()
        {
            _playerController = GameObject.FindWithTag("Player").GetComponent<Player.Controller>();
            SetUpHealthBarObservers();
            
            _playerInventoryManager = GameObject.FindWithTag("Player").GetComponent<InventoryManager>();
            UpdateFullInventory();
            SetUpInventoryObservers();
        }
        
        //Enables all the listeners needed for the HUD inventory
        private void SetUpInventoryObservers()
        {
            _playerInventoryManager.AddWeaponHUD += Inventory.AddToInventory;
            _playerInventoryManager.RemoveWeaponHUD += Inventory.RemoveFromInventory;
            _playerInventoryManager.ChangeSelectedWeaponHUD += Inventory.ChangeSelected;
        }
        
        //Enables all the listeners needed for the HUD health bar
        private void SetUpHealthBarObservers()
        {
            _playerController.UpdateHealthBarHUD += UpdateHealthBar;
            _playerController.ResetHealthBarHUD += ResetHealthBar;
        }

        //Populate the HUD inventory
        private void UpdateFullInventory()
        {
            Inventory.UpdateFullInventory(_playerInventoryManager.WeaponSlots);
        }
        
        //Modify the HUD health bar using animation
        private void UpdateHealthBar(int hitPoints)
        {
            HealthBar.UpdateHealthBar(hitPoints);
        }
        
        //Change the HUD health bar directly
        private void ResetHealthBar(int hitPoints)
        {
            HealthBar.ResetHealthBar(hitPoints);
        }
    }
}
