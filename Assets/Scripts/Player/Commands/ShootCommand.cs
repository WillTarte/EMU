namespace Player.Commands
{
    public class ShootCommand : Command
    {
        public override void Execute(Controller controller)
        {
            controller.InventoryManager.GetActiveWeapon()?.Shoot();
        }
    }   
}
