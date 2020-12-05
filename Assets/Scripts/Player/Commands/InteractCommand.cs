using Interactables;
using UnityEngine;
using WeaponsSystem.ScriptableObjects;

namespace Player.Commands
{
    public class InteractCommand : Command
    {
        public override void Execute(Controller controller)
        {
            base.Execute(controller);

            if (controller.NearestInteractable == null) return;
            if (controller.NearestInteractable.CompareTag("Chest"))
            {
                controller.NearestInteractable.GetComponent<ChestBehaviourScript>().Interact();
                controller.RemoveInteractable(controller.NearestInteractable);
            }
            else if (controller.NearestInteractable.CompareTag("Weapon"))
            {
                controller.InventoryManager.AddWeapon(controller.NearestInteractable);
                controller.RemoveInteractable(controller.NearestInteractable);
                
                if (controller.InventoryManager.GetActiveWeapon().WeaponData.WeaponName == WeaponName.Knife)
                {
                    controller.ChangeToKnifeAnimation();
                }
                else if (controller.InventoryManager.GetActiveWeapon().WeaponData.WeaponName == WeaponName.AssaultRifle)
                {
                    controller.ChangeToBigGunAnimation();
                }
                else if (controller.InventoryManager.GetActiveWeapon().WeaponData.WeaponName == WeaponName.Shotgun)
                {
                    controller.ChangeToShotgunAnimation();
                }
                else if (controller.InventoryManager.GetActiveWeapon().WeaponData.WeaponName == WeaponName.Sniper)
                {
                    controller.ChangeToSniperAnimation();
                }
            }
        }
    }
}