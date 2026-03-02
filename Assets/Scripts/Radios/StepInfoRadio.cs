using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/StepInfoUpdate")]
public class StepInfoRadio : ScriptableObject
{
    public event Action<string,string,int, int> OnStepInfoUpdate;
    
    public void RaiseStepInfoUpdate(string info,string progress, int stepIndex, int totalSteps)
    {
        OnStepInfoUpdate?.Invoke(info, progress, stepIndex, totalSteps);
    }

    private void OnDisable()
    {
        OnStepInfoUpdate = null;
    }
}