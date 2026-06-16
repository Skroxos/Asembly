using DroneAssembly.Radios;
using TMPro;
using UnityEngine;

namespace DroneAssembly.StepManager
{
    public class StepManagerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepDescriptionText;
        [SerializeField] private TextMeshProUGUI stepMissionProgressText;
        [SerializeField] private TextMeshProUGUI stepCounterText;
        [SerializeField] private StepInfoRadio uiChannel;

        private void OnEnable()
        {
            uiChannel.OnEventRaised += UpdateUI;
        }

        private void OnDisable()
        {
            uiChannel.OnEventRaised -= UpdateUI;
        }

        private void UpdateUI(StepInfoData infoData)
        {
            stepDescriptionText.SetText(infoData.info);
            stepMissionProgressText.SetText(infoData.progress);
            if (infoData.stepIndex > infoData.totalSteps)
            {
                stepCounterText.SetText("Procedure Completed!");
                stepCounterText.color = Color.green;
                stepDescriptionText.alpha = 0f;
            }
            else
            {
                stepCounterText.SetText("Step {0} of {1}", infoData.stepIndex, infoData.totalSteps);
            }
        }
    }
}