using WeaponsSystem.ScriptableObjects;

namespace Player.Commands
{
    public class ShootCommand : Command
    {
        public override void Execute(Controller controller)
        {
            controller.InventoryManager.GetActiveWeapon()?.Shoot();

            if (controller.InventoryManager.GetActiveWeapon()?.WeaponData.WeaponName == WeaponName.Knife)
            {
                controller.Animator.SetTrigger("Knife");
            }
        }
    }   
}
