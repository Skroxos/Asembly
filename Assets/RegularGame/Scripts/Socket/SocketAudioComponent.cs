using DroneAssembly.Audio;
using DroneAssembly.Radios;
using DroneAssembly.Radios.GeneralRadios;
using UnityEngine;

namespace DroneAssembly.Socket
{
    public class SocketAudioComponent : MonoBehaviour
    {
        [SerializeField] private AudioConfig snapSound;
        [SerializeField] private AudioClipRadio audioClipRadio;
        [SerializeField] private SimpleEventRadio snapEventRadio;
        


      

        private void OnEnable()
        {
            snapEventRadio.OnRaised += HandlePartSnapped;
        }


        private void OnDisable()
        {
            snapEventRadio.OnRaised -= HandlePartSnapped;
        }

        private void HandlePartSnapped()
        {
            if (snapSound != null && snapSound.audioClips.Length > 0) audioClipRadio.RaiseEvent(snapSound);
        }
    }
}