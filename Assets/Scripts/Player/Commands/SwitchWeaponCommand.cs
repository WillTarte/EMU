using UnityEngine;
using WeaponsSystem.ScriptableObjects;

namespace Player.Commands
{
    public class SwitchWeaponCommand : Command
    {
        public KeyCode keyPressed;

        public SwitchWeaponCommand(KeyCode key)
        {
            keyPressed = key;
        }

        public override void Execute(Controller controller)
        {
            base.Execute(controller);

            controller.InventoryManager.SwitchActiveWeapon(keyPressed);
            
            if (controller.InventoryManager.GetActiveWeapon().WeaponData.WeaponName == WeaponName.Knife) {
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