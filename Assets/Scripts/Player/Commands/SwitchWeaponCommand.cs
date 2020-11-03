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

            controller.InventoryManager.SwitchActiveWeapon(keyPressed);
        }

    }
}