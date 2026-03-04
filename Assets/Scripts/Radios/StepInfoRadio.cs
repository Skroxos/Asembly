using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/StepInfoUpdate")]
public class StepInfoRadio : GeneralRadio<StepInfoData>
{
}

[Serializable]
public struct StepInfoData
{
    public string info;
    public string progress;
    public int stepIndex;
    public int totalSteps;

    public StepInfoData(string info, string progress, int stepIndex, int totalSteps)
    {
        this.info = info;
        this.progress = progress;
        this.stepIndex = stepIndex;
        this.totalSteps = totalSteps;
    }
}