using Player.Commands;
using UnityEngine;

public class InputHandler
{
    private readonly Command _jumpCmd = new JumpCommand();
    private readonly Command _shootCmd = new ShootCommand();

    public Command HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return _jumpCmd;
        else return null;
    }
}