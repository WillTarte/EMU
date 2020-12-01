using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            Indestructibles.LastLevel = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame()
        {
            Debug.Log("Game has been quit");
            Application.Quit();
        }
    }
}
