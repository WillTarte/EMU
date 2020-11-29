using Player.Commands;
using UnityEngine;

namespace Player
{
    public class InputHandler
    {
        private readonly Command _jumpCmd = new JumpCommand();
        private readonly Command _shootCmd = new ShootCommand();
        private readonly Command _reloadCmd = new ReloadCommand();
        private readonly Command _interactCommand = new InteractCommand();
        private readonly Command _throwCommand = new ThrowCommand();

        public Command HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space)) return _jumpCmd;
            else if (Input.GetKeyDown(KeyCode.R)) return _reloadCmd;
            else if (Input.GetKey(KeyCode.K)) return _shootCmd;
            else if (Input.GetKeyDown(KeyCode.E)) return _interactCommand;
            else if (Input.GetKeyDown(KeyCode.Alpha1)) return new SwitchWeaponCommand(KeyCode.Alpha1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) return new SwitchWeaponCommand(KeyCode.Alpha2);
            else if (Input.GetKeyDown(KeyCode.G)) return _throwCommand;
            else return null;
        }
    }
}