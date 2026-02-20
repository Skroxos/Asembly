

[System.Serializable]
public class StepRequirement
{
    public SocketIDSO requiredPartID;
    public int amountRequired;
    [System.NonSerialized] public int currentAmount;
    
    public bool IsComplete => currentAmount >= amountRequired;
}

