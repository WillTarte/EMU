using UnityEngine;

namespace Player.Commands
{
    public class PickupCommand : Command
    {
        public override void Execute(Controller controller)
        {
            base.Execute(controller);

            if (controller.NearestPickup != null)
            {
                if (controller.InventoryManager.AddWeapon(controller.NearestPickup))
                {
                    controller.NearestPickup = null;
                }
            }
        }
    }
}