using System;
using DroneAssembly.Radios.GeneralRadios;
using UnityEngine;

namespace DroneAssembly.Radios
{
    [CreateAssetMenu(menuName = "Event/StepInfoUpdate")]
    public class StepInfoRadio : GeneralRadio<StepInfoData>
    {
    }

    [Serializable]
    public readonly struct StepInfoData
    {
        public readonly string info;
        public readonly string progress;
        public readonly int stepIndex;
        public readonly int totalSteps;

        public StepInfoData(string info, string progress, int stepIndex, int totalSteps)
        {
            this.info = info;
            this.progress = progress;
            this.stepIndex = stepIndex;
            this.totalSteps = totalSteps;
        }
    }
}