using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    
    public void SetVolume(int vol)
    {
        PlayerPrefs.SetInt("volume", vol);
        audioMixer.SetFloat("MasterVolume", (-80 + vol * 8)); // volume goes from 0 to -80
    }
}