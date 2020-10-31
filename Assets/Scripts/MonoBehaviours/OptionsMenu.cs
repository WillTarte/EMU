using UnityEngine;
using UnityEngine.Audio;

namespace MonoBehaviours
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
    
        public void SetVolume(int vol)
        {
            audioMixer.SetFloat("MasterVolume", (-80 + vol * 8)); // volume goes from 0 to -80
        }
    }
}
