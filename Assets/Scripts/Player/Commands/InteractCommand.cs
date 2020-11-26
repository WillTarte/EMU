using Interactables;

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
            }
        }
    }
}