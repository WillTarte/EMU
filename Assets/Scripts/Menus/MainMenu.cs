using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        private void Awake()
        {
            PlayerPrefs.SetInt("volume", 7);
        }

        public void PlayGame()
        {
            Indestructibles.LastLevel = 1;
            Indestructibles.respawnPos = Indestructibles.defaultSpawns[Indestructibles.LastLevel - 1];
            SceneManager.LoadScene(Indestructibles.LastLevel);
        }

        public void QuitGame()
        {
            Debug.Log("Game has been quit");
            Application.Quit();
        }
    }
}

