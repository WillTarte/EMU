using UnityEngine;

namespace Player.Commands
{
    public class PickupCommand : Command
    {
        public override void Execute(Controller controller)
        {
            base.Execute(controller);
            
            Debug.Log("Pickup Command");

            if (controller.NearestPickup != null)
            {
                controller.InventoryManager.AddWeapon(controller.NearestPickup);
            }
        }
    }
}