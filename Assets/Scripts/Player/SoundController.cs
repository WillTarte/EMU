using System;
using System.Collections.Generic;
using System.Linq;
using Player.Commands;
using Player.States;
using UnityEngine;

namespace Player
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioClips;

        private readonly Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
        private Controller _characterController;
        private AudioSource _audioSource;

        private void Awake()
        {
            _characterController = GetComponent<Controller>();
            _audioSource = GetComponent<AudioSource>();

            if (_characterController != null)
            {
                _characterController.OnStateChanged += PlaySoundOnStateChanged;
                _characterController.OnCommandInputted += PlaySoundOnCommand;
            }
            else
            {
                Debug.Log("Unable to subscribe to state changes due to player not having a character controller");
            }

            foreach (var clip in audioClips)
            {
                _audioClips.Add(clip.name, clip);
            }
        }

        private void OnDestroy()
        {
            if (_characterController != null)
            {
                _characterController.OnStateChanged -= PlaySoundOnStateChanged;
                _characterController.OnCommandInputted -= PlaySoundOnCommand;
            }
        }

        private void PlaySoundOnStateChanged(BaseState newState)
        {
            switch (newState)
            {
                case RunState _:
                    PlaySound("playerRunAudioClip", true);
                    break;
                case FallState _:
                case IdleState _:
                    if (_audioSource.clip.name.Equals("playerRunAudioClip"))
                    {
                        _audioSource.Stop();
                    }
                    break;
                case RollState _:
                    PlaySound("playerRollAudioClip", true); //todo
                    break;
            }
        }

        private void PlaySoundOnCommand(Command cmd)
        {
            switch (cmd)
            {
                case JumpCommand _:
                    if (_characterController.CurrentState is IdleState || _characterController.CurrentState is RunState)
                    {
                        PlaySound("playerJumpAudioClip", false);
                    }
                    break;
                case SwitchWeaponCommand _:
                    PlaySound("playerSwitchWeaponAudioClip", false);
                    break;
                case ThrowCommand _:
                    if (_characterController.CurrentState is IdleState ||
                        _characterController.CurrentState is RunState || _characterController.CurrentState is FallState)
                    {
                        PlaySound("playerThrowAudioClip", false);
                    }
                    break;
            }
        }

        private void PlaySound(string clipName, bool loop)
        {
            if (_audioClips.TryGetValue(clipName, out var clip))
            {
                _audioSource.clip = clip;
                _audioSource.loop = loop;
                _audioSource.volume = PlayerPrefs.GetInt("volume") / 10.0f;
                _audioSource.Play();
            }
            else
            {
                Debug.Log("Could not find audio clip for name " + clipName);
            }
        }
    }
}
