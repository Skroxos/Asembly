using System.Collections.Generic;
using NUnit.Framework;

public class StepLogicTest
{
    [Test]
    public void StepRequirement_WhenAmountMet_IsCompletedReturnsTrue()
    {
        var requirement = new StepRequirement
        {
            requiredPartID = null,
            amountRequired = 3,
            currentAmount = 3
        };

        var isComplete = requirement.IsComplete;

        Assert.IsTrue(isComplete);
    }

    [Test]
    public void Step_WhenAllRequirementsMet_StepIsCompletedReturnsTrue()
    {
        var step = new Step();
        step.requiredParts = new List<StepRequirement>
        {
            new() { requiredPartID = null, amountRequired = 2, currentAmount = 2 },
            new() { requiredPartID = null, amountRequired = 4, currentAmount = 4 }
        };
        var result = step.IsCompleted();
        Assert.IsTrue(result);
    }

    [Test]
    public void Step_WhenNotAllRequirementsMet_StepIsCompletedReturnsFalse()
    {
        var step = new Step();
        step.requiredParts = new List<StepRequirement>
        {
            new() { requiredPartID = null, amountRequired = 2, currentAmount = 2 },
            new() { requiredPartID = null, amountRequired = 4, currentAmount = 3 }
        };
        var result = step.IsCompleted();
        Assert.IsFalse(result);
    }

    [Test]
    public void Step_WhenCurrentAmountExceedsRequired_StepIsCompletedReturnsTrue()
    {
        var step = new Step();
        step.requiredParts = new List<StepRequirement>
        {
            new() { requiredPartID = null, amountRequired = 2, currentAmount = 3 },
            new() { requiredPartID = null, amountRequired = 4, currentAmount = 5 }
        };
        var result = step.IsCompleted();
        Assert.IsTrue(result);
    }

    [Test]
    public void Step_WithEmptyRequirementsList_ReturnsTrue()
    {
        var step = new Step { requiredParts = new List<StepRequirement>() };
        Assert.IsTrue(step.IsCompleted());
    }
}