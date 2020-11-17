namespace Player.Commands
{
    public class ThrowCommand : Command
    {
        public override void Execute(Controller controller)
        {
            controller.InventoryManager.GetThrowableWeapon()?.Shoot();
        }
    }
}