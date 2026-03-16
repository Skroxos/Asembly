using DroneAssembly.Radios;
using UnityEngine;

namespace DroneAssembly.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClipRadio audioClipRadio;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            audioClipRadio.OnEventRaised += HandleAudioClipUpdate;
        }

        private void OnDisable()
        {
            audioClipRadio.OnEventRaised -= HandleAudioClipUpdate;
        }

        private void HandleAudioClipUpdate(AudioConfig config)
        {
            var clip = config.audioClips[Random.Range(0, config.audioClips.Length)];
            audioSource.pitch = Random.Range(config.minPitch, config.maxPitch);
            audioSource.volume = config.volume;
            audioSource.PlayOneShot(clip, config.volume);
        }
    }
}