using DroneAssembly.Procedure;
using DroneAssembly.Radios;
using DroneAssembly.Radios.GeneralRadios;
using DroneAssembly.Socket;
using DroneAssembly.Validator;
using System.Collections.Generic;
using System.Text;
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

        // Preallocate Stuff
        private readonly StringBuilder _progressBuilder = new StringBuilder(100);
        private readonly List<SocketIDSO> _allowedSocketsBuffer = new List<SocketIDSO>(20);
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
        }

        private void HandlePartSnapped(SocketIDSO obj)
        {
            if (_currentStep == null) return;

            StepRequirement requirement = null;
            foreach (var req in _currentStep.requiredParts)
            {
                if (req.requiredPartID == obj)
                {
                    requirement = req;
                    break;
                }
            }

            if (requirement != null)
            {
                if (requirement.amountRequired > requirement.currentAmount)
                {
                    requirement.currentAmount++;
                    BroadCastStepInfo();
                }

                if (_currentStep.IsCompleted())
                {
                    AdvanceStep();
                }
            }
        }

        private void AdvanceStep()
        {
            _currentStepIndex++;
            if (_currentStepIndex < procedure.steps.Count)
            {
               LoadStep(_currentStepIndex);
            }
            else
            {
                _currentStep = null;
                if (procedureCompleteRadio != null)
                {
                    procedureCompleteRadio.RaiseEvent();
                }
                BroadCastStepInfo();
                UpdateAllowedSockets();
            }

        }

        private void LoadStep(int index)
        {
            _currentStep = procedure.steps[index];
            foreach (var req in _currentStep.requiredParts)
            {
                req.currentAmount = 0;
            }
            if(spawnPartRadio != null) 
            {
                spawnPartRadio.RaiseEvent(_currentStep.requiredParts);
            }

            BroadCastStepInfo();
            UpdateAllowedSockets();
        }

        private void UpdateAllowedSockets()
        {
            if (stepValidation == null || _currentStep == null) return;
              
            _allowedSocketsBuffer.Clear();
            foreach (var req in _currentStep.requiredParts)
            {
              _allowedSocketsBuffer.Add(req.requiredPartID);
            }
            stepValidation.UpdateAllowedSockets(_allowedSocketsBuffer);

        }

        private void BroadCastStepInfo()
        {
            if (uiChannel == null) return;

                var description = "Done";
                _progressBuilder.Clear();
            if (_currentStep != null)
            {
                    description = _currentStep.description;
                    foreach (var req in _currentStep.requiredParts)
                    {
                        _progressBuilder.AppendFormat("{0}: {1}/{2}\n", req.requiredPartID.name, req.currentAmount, req.amountRequired);
                    }
            }

                uiChannel.RaiseEvent(new StepInfoData(description, _progressBuilder.ToString(), _currentStepIndex + 1, procedure.steps.Count));
            
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