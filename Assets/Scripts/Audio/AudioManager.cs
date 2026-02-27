using UnityEngine;

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
        audioClipRadio.OnAudioClipUpdate += HandleAudioClipUpdate;
    }

    private void OnDisable()
    {
        audioClipRadio.OnAudioClipUpdate -= HandleAudioClipUpdate;
    }

    private void HandleAudioClipUpdate(AudioConfig config)
    {
      AudioClip clip = config.audioClips[Random.Range(0, config.audioClips.Length)];
      audioSource.pitch = Random.Range(config.minPitch, config.maxPitch);
      audioSource.volume = config.volume;
      audioSource.PlayOneShot(clip, config.volume);
    }
}