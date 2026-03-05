using System.Linq;
using DroneAssembly.Procedure;
using DroneAssembly.Radios;
using DroneAssembly.Radios.GeneralRadios;
using DroneAssembly.Socket;
using DroneAssembly.Validator;
using UnityEngine;

namespace DroneAssembly.StepManager
{
    public class StepManager : MonoBehaviour
    {
        [SerializeField] private ProcedureSO procedure;
        [SerializeField] private SocketStepValidationSO stepValidation;
        [SerializeField] private EventRadio eventRadio;
        [SerializeField] private StepInfoRadio uiChannel;
        [SerializeField] private SpawnPartRadioSO spawnPartRadio;
        [SerializeField] private SimpleEventRadio procedureCompleteRadio;

        private Step _currentStep;
        private int _currentStepIndex;
    
        private void Start()
        {
            InitializeSteps();
        }

        private void OnEnable()
        {
            eventRadio.OnEventRaised += HandlePartSnapped;
        }

        private void OnDisable()
        {
            eventRadio.OnEventRaised -= HandlePartSnapped;
        }

        private void InitializeSteps()
        {
            if (procedure != null && procedure.steps.Count > 0)
            {
                LoadStep(_currentStepIndex);
                _currentStep = procedure.steps[0];
            }

            BroadCastStepInfo();
        }

        private void HandlePartSnapped(SocketIDSO obj)
        {
            if (_currentStep == null) return;

            var matchingRequirement = _currentStep.requiredParts.FirstOrDefault(req => req.requiredPartID == obj);

            if (matchingRequirement != null)
            {
                if (matchingRequirement.amountRequired > matchingRequirement.currentAmount)
                {
                    matchingRequirement.currentAmount++;
                    BroadCastStepInfo();
                }

                if (_currentStep.IsCompleted()) AdvanceStep();
            }
        }

        private void AdvanceStep()
        {
            _currentStepIndex++;
            if (_currentStepIndex < procedure.steps.Count)
            {
                _currentStep = procedure.steps[_currentStepIndex];

                foreach (var req in _currentStep.requiredParts) req.currentAmount = 0;
                if(spawnPartRadio != null) spawnPartRadio.RaiseEvent(_currentStep.requiredParts);
            }
            else
            {
                _currentStep = null;
                if (procedureCompleteRadio != null) procedureCompleteRadio.RaiseEvent();
            }

            BroadCastStepInfo();
            UpdateAllowedSockets();
        }

        private void LoadStep(int index)
        {
            _currentStep = procedure.steps[index];
            foreach (var req in _currentStep.requiredParts) req.currentAmount = 0;
            if(spawnPartRadio != null) spawnPartRadio.RaiseEvent(_currentStep.requiredParts);

            BroadCastStepInfo();
            UpdateAllowedSockets();
        }

        private void UpdateAllowedSockets()
        {
            if (stepValidation != null && _currentStep != null)
            {
                var allowedSockets = _currentStep.requiredParts.Select(req => req.requiredPartID).ToList();
                stepValidation.UpdateAllowedSockets(allowedSockets);
            }
        }

        private void BroadCastStepInfo()
        {
            if (uiChannel != null)
            {
                var description = "Done";
                var progress = "";
                if (_currentStep != null)
                {
                    description = _currentStep.description;
                    foreach (var req in _currentStep.requiredParts)
                        progress += $"{req.requiredPartID.name}: {req.currentAmount}/{req.amountRequired}\n";
                }

                uiChannel.RaiseEvent(new StepInfoData(description, progress, _currentStepIndex + 1, procedure.steps.Count));
            }
        }


#if UNITY_EDITOR
        public void AdvanceStepDebug()
        {
            AdvanceStep();
        }

        public void ResetStepsDebug()
        {
            _currentStepIndex = 0;
            LoadStep(_currentStepIndex);
        }

        public void JumpToStepDebug(int stepIndex)
        {
            if (stepIndex >= 0 && stepIndex <= procedure.steps.Count)
            {
                _currentStepIndex = stepIndex - 1;
                LoadStep(_currentStepIndex);
            }
        }

        public int GetTotalStepsDebug()
        {
            if (procedure != null && procedure.steps != null) return procedure.steps.Count;
            return 0;
        }
#endif
    }
}