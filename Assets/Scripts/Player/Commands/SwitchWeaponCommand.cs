using UnityEngine;

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
            
            Debug.Log("Pickup Command");

            if (controller.NearestPickup != null)
            {
                controller.InventoryManager.SwitchActiveWeapon(keyPressed);
            }
        }

    }
}