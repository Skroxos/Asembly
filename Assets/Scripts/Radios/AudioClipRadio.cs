using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Event/AudioClipRadio")]
public class AudioClipRadio : ScriptableObject
{
    public event Action<AudioConfig> OnAudioClipUpdate;
    public void RaiseAudioClipUpdate(AudioConfig config)
    {
        OnAudioClipUpdate?.Invoke(config);
    }

    private void OnDisable()
    {
        OnAudioClipUpdate = null;
    }
}

