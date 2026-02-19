using System.Linq;
using UnityEngine;
public class StepManager : MonoBehaviour
{
    [SerializeField] private ProcedureSO procedure;
    private int currentStepIndex = 0;
    private Step currentStep;
    [SerializeField] private EventRadio eventRadio;
    [SerializeField] private StepInfoRadio uiChannel;

    private void Awake()
    {
        InitializeSteps();
    }

    private void InitializeSteps()
    {
        if (procedure != null && procedure.steps.Count > 0)
        {
            LoadStep(currentStepIndex);
            currentStep = procedure.steps[0];
        }
        BroadCastStepInfo();
    }

    private void OnEnable()
    {
        eventRadio.OnSnap += HandlePartSnapped;
    }

    private void OnDisable()
    {
        eventRadio.OnSnap -= HandlePartSnapped;
    }

    private void HandlePartSnapped(SocketIDSO obj)
    {
        if (currentStep == null) return;
        
        var matchingRequirement = currentStep.requiredParts.FirstOrDefault(req => req.requiredPartID == obj);
        
        if (matchingRequirement != null)
        {
            if (matchingRequirement.amountRequired > matchingRequirement.currentAmount)
            {
                matchingRequirement.currentAmount++;
                BroadCastStepInfo();
            }
            
            if (currentStep.IsCompleted())
            {
                AdvanceStep();
            }
        }
        
    }
    
    private void AdvanceStep()
    {
        currentStepIndex++;
        if (currentStepIndex < procedure.steps.Count)
        {
            currentStep = procedure.steps[currentStepIndex];
        }
        
        BroadCastStepInfo();
        
    }
    
    private void LoadStep(int index)
    {
        currentStep = procedure.steps[index];
        foreach (var req in currentStep.requiredParts)
        {
            req.currentAmount = 0;
        }
        BroadCastStepInfo();
    }
    
    private void BroadCastStepInfo()
    {
        if (uiChannel != null)
        {
            string description = "Done";
            string progress = "";
            if (currentStep != null)
            {
                description = currentStep.description;
                foreach (var req in currentStep.requiredParts)
                {
                    progress += $"{req.requiredPartID.name}: {req.currentAmount}/{req.amountRequired}\n";
                }
            }
            uiChannel.RaiseStepInfoUpdate($"{description}\n{progress}", currentStepIndex, procedure.steps.Count);
        }
    }
}