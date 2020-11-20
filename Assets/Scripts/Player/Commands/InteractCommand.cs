using MonoBehaviours;
using UnityEngine;

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
                controller.NearestInteractable = null;
            }
            else if (controller.NearestInteractable.CompareTag("Weapon"))
            {

                if (controller.InventoryManager.AddWeapon(controller.NearestInteractable))
                {
                    controller.NearestInteractable = null;
                }
            }
        }
    }
}