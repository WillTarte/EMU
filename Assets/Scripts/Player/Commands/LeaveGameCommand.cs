using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player.Commands
{
    public class LeaveGameCommand : Command
    {
        public override void Execute(Controller controller)
        {
            SceneManager.LoadScene(0);
        }
    }
}
