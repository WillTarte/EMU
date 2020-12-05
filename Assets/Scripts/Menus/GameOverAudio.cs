using UnityEngine;

namespace Menus
{
    public class GameOverAudio : MonoBehaviour
    {
        private void Awake()
        {
            var audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = PlayerPrefs.GetInt("volume") / 10.0f;
            }
        }
    }
}
