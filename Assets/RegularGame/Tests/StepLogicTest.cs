using System.Collections.Generic;
using DroneAssembly.StepManager;
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
    
    
    [Test]
    public void StepRequirement_WhenAmountNotMet_IsCompletedReturnsFalse()
    {
        var requirement = new StepRequirement
        {
            requiredPartID = null,
            amountRequired = 3,
            currentAmount = 1
        };

        Assert.IsFalse(requirement.IsComplete);
    }

    [Test]
    public void StepRequirement_WhenAmountIsZero_IsCompletedReturnsTrue()
    {
        var requirement = new StepRequirement
        {
            requiredPartID = null,
            amountRequired = 0,
            currentAmount = 0
        };

        Assert.IsTrue(requirement.IsComplete);
    }

    [Test]
    public void Step_WhenRequiredPartsListIsEmpty_IsCompletedReturnsTrue()
    {
        var step = new Step
        {
            requiredParts = new List<StepRequirement>()
        };

        Assert.IsTrue(step.IsCompleted());
    }

    [Test]
    public void Step_WhenOneRequirementNotMet_IsCompletedReturnsFalse()
    {
        var step = new Step
        {
            requiredParts = new List<StepRequirement>
            {
                new() { requiredPartID = null, amountRequired = 1, currentAmount = 1 },
                new() { requiredPartID = null, amountRequired = 3, currentAmount = 2 },
                new() { requiredPartID = null, amountRequired = 1, currentAmount = 1 }
            }
        };

        Assert.IsFalse(step.IsCompleted());
    }
    
}