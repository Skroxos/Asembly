using DroneAssembly.Audio;
using DroneAssembly.Radios;
using UnityEngine;

namespace DroneAssembly.Socket
{
    public class SocketAudioComponent : MonoBehaviour
    {
        [SerializeField] private AudioConfig snapSound;
        [SerializeField] private AudioClipRadio audioClipRadio;
        private ISocketEvents _socketEvents;

        private void Awake()
        {
            _socketEvents = GetComponent<ISocketEvents>();
            if (_socketEvents == null)
            {
                Debug.LogError("SocketAudioComponent requires a component that implements ISocketEvents.");
            }
        }

        private void OnEnable()
        {
            if (_socketEvents != null)
            {
                _socketEvents.OnPartSnapped += HandlePartSnapped;
            }
        }


        private void OnDisable()
        {
            if (_socketEvents != null)
            {
                _socketEvents.OnPartSnapped -= HandlePartSnapped;
            }
        }

        private void HandlePartSnapped()
        {
            if (snapSound != null && snapSound.audioClips.Length > 0) audioClipRadio.RaiseEvent(snapSound);
        }
    }
}