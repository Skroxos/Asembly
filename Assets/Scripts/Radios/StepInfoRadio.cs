using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/StepInfoUpdate")]
public class StepInfoRadio : ScriptableObject
{
    public event Action<string, int, int> OnStepInfoUpdate;
    
    public void RaiseStepInfoUpdate(string info, int stepIndex, int totalSteps)
    {
        OnStepInfoUpdate?.Invoke(info, stepIndex, totalSteps);
    }

    private void OnDisable()
    {
        OnStepInfoUpdate = null;
    }
}