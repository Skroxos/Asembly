using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        stepDescriptionText.text = infoData.info;
        stepMissionProgressText.text = infoData.progress;
        if (infoData.stepIndex > infoData.totalSteps)
        {
            stepCounterText.text = "Procedure Completed!";
            stepCounterText.color = Color.green;
            stepDescriptionText.alpha = 0f;
        }
        else
        {
            stepCounterText.text = $"Step {infoData.stepIndex} of {infoData.totalSteps}";
        }
    }
}