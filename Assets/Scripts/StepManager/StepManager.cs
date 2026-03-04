using System;
using System.Linq;
using UnityEngine;
public class StepManager : MonoBehaviour
{
    [SerializeField] private ProcedureSO procedure;
    [SerializeField] private SocketStepValidationSO stepValidation;
    private int currentStepIndex = 0;
    private Step currentStep;
    [SerializeField] private EventRadio eventRadio;
    [SerializeField] private StepInfoRadio uiChannel;
    [SerializeField] private DeskSpawner deskSpawner;
    
    private void Start()
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
        eventRadio.OnEventRaised += HandlePartSnapped;
    }

    private void OnDisable()
    {
        eventRadio.OnEventRaised -= HandlePartSnapped;
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

            foreach (var req in currentStep.requiredParts)
            {
                req.currentAmount = 0;
            }
            deskSpawner.SpawnParts(currentStep.requiredParts);
        }

        BroadCastStepInfo();
        UpdateAllowedSockets();
        
    }
    
    private void LoadStep(int index)
    {
        currentStep = procedure.steps[index];
        foreach (var req in currentStep.requiredParts)
        {
            req.currentAmount = 0;
        }
        deskSpawner.SpawnParts(currentStep.requiredParts);
        
        BroadCastStepInfo();
        UpdateAllowedSockets();
    }
    
    private void UpdateAllowedSockets()
    {
        if (stepValidation != null && currentStep != null)
        {
            var allowedSockets = currentStep.requiredParts.Select(req => req.requiredPartID).ToList();
            stepValidation.UpdateAllowedSockets(allowedSockets);
        }
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
            uiChannel.RaiseEvent(new StepInfoData(description, progress, currentStepIndex + 1, procedure.steps.Count));
        }
    }
    
    
    #if UNITY_EDITOR
    public void AdvanceStepDebug() =>  AdvanceStep();

    public void ResetStepsDebug()
    {
        currentStepIndex = 0;
        LoadStep(currentStepIndex);
    }
    
    public void JumpToStepDebug(int stepIndex)
    {
        if (stepIndex >= 0 && stepIndex <= procedure.steps.Count)
        {
            currentStepIndex = stepIndex - 1;
            LoadStep(currentStepIndex);
        }
    }

    public int GetTotalStepsDebug()
    {
        if (procedure != null && procedure.steps != null)
        {
            return procedure.steps.Count;
        }
        return 0;
    }
    #endif
    
}