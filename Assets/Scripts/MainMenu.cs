using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviours
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame()
        {
            Debug.Log("Game has been quit");
            Application.Quit();
        }
    }
}
