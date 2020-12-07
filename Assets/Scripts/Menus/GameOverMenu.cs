using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class GameOverMenu : MonoBehaviour
    {
       
        public void TryAgain()
        {
            SceneManager.LoadScene(Indestructibles.LastLevel);
        }

        public void QuitGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
