using UnityEngine;
using UnityEngine.UI;

namespace DroneAssembly.GamePlaySettings
{
    public class VolumeHandler : MonoBehaviour
    {
        [SerializeField] private Slider audioVolumeSlider;
        [SerializeField] private AudioListener audioListener;
        
        private void Start()
        {
            audioVolumeSlider.maxValue = 1f;
            audioVolumeSlider.value = AudioListener.volume;
            audioVolumeSlider.onValueChanged.AddListener(OnAudioVolumeChanged);
        }

        private void OnAudioVolumeChanged(float volume)
        {
            AudioListener.volume = volume;
        }
    }
}
