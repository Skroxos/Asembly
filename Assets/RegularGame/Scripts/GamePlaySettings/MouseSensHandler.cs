using DroneAssembly.Player.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace DroneAssembly.GamePlaySettings
{
   public class MouseSensHandler : MonoBehaviour
   {
      [SerializeField] private Slider mouseSensitivitySlider;
      [SerializeField] private PlayerConfig playerConfig;
   
      private void Start()
      {
         mouseSensitivitySlider.maxValue = 20f;
         mouseSensitivitySlider.value = playerConfig.MouseSensitivity;
         mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
      }
   
      private void OnMouseSensitivityChanged(float value)
      {
         playerConfig.MouseSensitivity = value;
      }
   
  
   }
}
