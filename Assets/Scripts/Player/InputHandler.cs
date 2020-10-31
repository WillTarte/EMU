using Player;
using Player.Commands;
using UnityEngine;

public class InputHandler
{
    private readonly Command _jumpCmd = new JumpCommand();
    private readonly Command _rollCmd = new RollCommand();
    private readonly Command _shootCmd = new ShootCommand();

    public Command HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return _jumpCmd;
        if (Input.GetKeyDown(KeyCode.LeftShift)) return _rollCmd;

        return null;
    }
}