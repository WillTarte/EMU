using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class GameWon : MonoBehaviour
    {
        public void TryAgain()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
