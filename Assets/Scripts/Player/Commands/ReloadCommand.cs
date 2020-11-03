namespace Player.Commands
{
    public class ReloadCommand : Command
    {
        public override void Execute(Controller controller)
        {
            controller.InventoryManager.GetActiveWeapon()?.Reload();
        }
    }   
}