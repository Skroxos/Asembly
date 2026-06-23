using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DroneAssembly.StepManager
{
    [Serializable]
    public class Step
    {
        [TextArea] public string description;

        public List<StepRequirement> requiredParts;

        public bool IsCompleted()
        {
           for (int i = 0; i < requiredParts.Count; i++)
            {
                if (!requiredParts[i].IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }
}