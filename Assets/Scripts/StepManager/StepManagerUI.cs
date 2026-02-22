using UnityEngine;
using TMPro;

public class StepManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepDescriptionText;
    [SerializeField] private TextMeshProUGUI stepCounterText;
    [SerializeField] private StepInfoRadio uiChannel;
    
    private void OnEnable()
    {
        uiChannel.OnStepInfoUpdate += UpdateUI;
    }
    
    private void OnDisable()
    {
        uiChannel.OnStepInfoUpdate -= UpdateUI;
    }

    private void UpdateUI(string description, int stepIndex, int totalSteps)
    {
        stepDescriptionText.text = description;
        if (stepIndex >= totalSteps)
        {
            stepCounterText.text = "Procedure Completed!";
            stepCounterText.color = Color.green;
            stepDescriptionText.alpha = 0f;
        }
        else
        {
            stepCounterText.text = $"Step {stepIndex} of {totalSteps}";
        }
      
    }
}