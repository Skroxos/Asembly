using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Step
{
    [TextArea] public string description;
    
    public List<StepRequirement> requiredParts;

    public bool IsCompleted()
    {
        return requiredParts.All(req => req.IsComplete);
    }
}