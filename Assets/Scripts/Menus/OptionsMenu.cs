using UnityEngine;
using UnityEngine.Audio;

namespace Menus
{
    public class OptionsMenu : MonoBehaviour
    {
        public void SetVolume(int vol)
        {
            PlayerPrefs.SetInt("volume", vol);
            var audioGO = GameObject.Find("MenuThemeSong");
            if (audioGO != null)
            {
                var audioSource = audioGO.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                   audioSource.volume = vol / 10.0f;
                }
            }
                
        }
    }
}