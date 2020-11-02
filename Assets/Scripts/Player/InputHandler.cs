using Player.Commands;
using UnityEngine;

public class InputHandler
{
    private readonly Command _jumpCmd = new JumpCommand();
    private readonly Command _shootCmd = new ShootCommand();
    private readonly Command _reloadCmd = new ReloadCommand();

    public Command HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return _jumpCmd;
        else if (Input.GetKeyDown(KeyCode.R)) return _reloadCmd;
        else if (Input.GetMouseButton(0)) return _shootCmd;
        else return null;
    }
}