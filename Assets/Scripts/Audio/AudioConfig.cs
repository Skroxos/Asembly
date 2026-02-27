using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    public AudioClip[] audioClips;
    
    [Range(0, 1)] public float volume = 0.5f;
    
    [Range(0, 2)] public float minPitch = 0.9f;
    [Range(0, 2)] public float maxPitch = 1.1f;
}