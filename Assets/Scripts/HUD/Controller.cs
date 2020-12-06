using Player;
using EnemySystem.Monobehaviours;
using UnityEngine;

namespace HUD
{
    public class Controller : MonoBehaviour
    {
        public HealthBar HealthBar;
        public BossHealthBar BossHealthBar;
        public Inventory Inventory;

        //Used to access hitPoints of player
        private Player.Controller _playerController;
        //Used to access hitPoints of boss
        private BossController _bossController;
        //Used to access the weapons owned by player
        private InventoryManager _playerInventoryManager;

        void Start()
        {
            _playerController = GameObject.FindWithTag("Player").GetComponent<Player.Controller>();
            SetUpHealthBarObservers();
            
            _bossController = GameObject.FindWithTag("Boss").GetComponent<BossController>();
            SetUpBossHealthBarObservers();
            
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
        
        //Enables all the listeners needed for the HUD boss health bar
        private void SetUpBossHealthBarObservers()
        {
            _bossController.UpdateBossHealthBarHUD += UpdateBossHealthBar;
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
        
        //Modify the HUD boss health bar
        private void UpdateBossHealthBar(int hitPoints)
        {
            BossHealthBar.UpdateBossHealthBar(hitPoints);
        }
        
    }
}
