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
        private Rigidbody2D _rigidbody;
        private AudioSource _audioSource;
        private bool _climbing;
        private bool _loopingClimbingAudioClip;

        private void Awake()
        {
            _characterController = GetComponent<Controller>();
            _audioSource = GetComponent<AudioSource>();
            _rigidbody = GetComponent<Rigidbody2D>();

            if (_characterController != null)
            {
                _characterController.OnStateChanged += PlaySoundOnStateChanged;
                _characterController.OnCommandInputted += PlaySoundOnCommand;
                _characterController.OnDamageTakenEvent += PlayDamageTakenAudioClip;
            }
            else
            {
                Debug.Log("Unable to subscribe to state changes due to player not having a character controller");
            }

            if (_audioSource != null)
            {
                _audioSource.volume = PlayerPrefs.GetInt("volume") / 10.0f;
            }
            
            foreach (var clip in audioClips)
            {
                _audioClips.Add(clip.name, clip);
            }
        }

        private void Update()
        {
            // check if not climbing
            if (!(_characterController.CurrentState is ClimbState))
            {
                if (_audioSource.clip != null && _audioSource.clip.name.Equals("playerClimbAudioClip"))
                {
                    _audioSource.Stop();
                }
                _climbing = false;
                _loopingClimbingAudioClip = false;
            }
            
            // if just started climbing
            if (_characterController.CurrentState is ClimbState && !_climbing)
            {
                _climbing = true;
            }

            // if climbing and actually moving
            if (_climbing && _rigidbody.velocity.y != 0 && !_loopingClimbingAudioClip)
            {
                PlaySound("playerClimbAudioClip", true);
                _loopingClimbingAudioClip = true;
            }
            else if (_rigidbody.velocity.y == 0)
            {
                if (_audioSource.clip != null && _audioSource.clip.name.Equals("playerClimbAudioClip"))
                {
                    _audioSource.Stop();
                }
                _loopingClimbingAudioClip = false;
            }
        }

        private void PlayDamageTakenAudioClip()
        {
            PlaySound("playerDamagedAudioClip", false);
        }

        private void OnDestroy()
        {
            if (_characterController != null)
            {
                _characterController.OnStateChanged -= PlaySoundOnStateChanged;
                _characterController.OnCommandInputted -= PlaySoundOnCommand;
                _characterController.OnDamageTakenEvent -= PlayDamageTakenAudioClip;
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
                    if (_audioSource.clip != null && _audioSource.clip.name.Equals("playerRunAudioClip"))
                    {
                        _audioSource.Stop();
                    }
                    break;
                case RollState _:
                    PlaySound("playerJumpAudioClip", false);
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
                        PlaySoundOneShot("playerJumpAudioClip");
                    }
                    break;
                case SwitchWeaponCommand _:
                    PlaySoundOneShot("playerSwitchWeaponAudioClip");
                    break;
            }
        }

        private void PlaySound(string clipName, bool loop)
        {
            if (_audioClips.TryGetValue(clipName, out var clip))
            {
                _audioSource.clip = clip;
                _audioSource.loop = loop;
                _audioSource.Play();
            }
            else
            {
                Debug.Log("Could not find audio clip for name " + clipName);
            }
        }

        private void PlaySoundOneShot(string clipName)
        {
            if (_audioClips.TryGetValue(clipName, out var clip))
            {
                AudioSource.PlayClipAtPoint(clip, transform.position, _audioSource.volume);
            }
            else
            {
                Debug.Log("Could not find audio clip for name " + clipName);
            }
        }
    }
}
