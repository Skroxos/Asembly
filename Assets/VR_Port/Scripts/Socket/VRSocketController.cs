using System;
using DroneAssembly.Radios;
using DroneAssembly.Socket;
using DroneAssembly.Validator;
using DroneAssembly.VR_Port.Part;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace DroneAssembly.VR_Port.Socket
{
    [RequireComponent(typeof(XRSocketInteractor))]
    public class VRSocketController : MonoBehaviour, IXRSelectFilter, ISocketValidation, IXRHoverFilter
    {
        [SerializeField] private SocketIDSO _typeIDSO;
        [SerializeField] private SocketStepValidationSO _socketStepValidationSO;
        [SerializeField] private EventRadio _eventRadio;
        
        private XRSocketInteractor _socket;
        private VRAssemblyPart _attachedPart;
        
        public event Action OnPartSnapped;
        
        private bool isOccupied;
        
        private void Awake()
        {
            _socket = GetComponent<XRSocketInteractor>();
            _socket.selectFilters.Add(this);
            _socket.hoverFilters.Add(this);
        }

        private void OnEnable()
        {
            _socket.selectEntered.AddListener(OnSellectEntered);
        }

        private void OnDisable()
        {
            _socket.selectEntered.RemoveAllListeners();
        }

        public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
        {
            if (isOccupied) return false;
            var part = interactable.transform.gameObject;
            if (!part.TryGetComponent(out VRAssemblyPart assemblyPart)) return false;
            if (assemblyPart.socketIDSO != _typeIDSO) return false;
            if (_socketStepValidationSO != null && !_socketStepValidationSO.IsSocketAllowed(_typeIDSO)) return false;
            return true;
        }

        public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
        {
            var part = interactable.transform.gameObject;
            if (_attachedPart != null && part == _attachedPart.gameObject) return true;
            if (isOccupied) return false;
            if (!part.TryGetComponent(out VRAssemblyPart assemblyPart)) return false;
            if (assemblyPart.socketIDSO != _typeIDSO) return false;
            if (_socketStepValidationSO != null && !_socketStepValidationSO.IsSocketAllowed(_typeIDSO)) return false;
            return true;
        }
        public bool canProcess => true;

        private void OnSellectEntered(SelectEnterEventArgs args)
        {
          var part = args.interactableObject.transform.gameObject.GetComponent<VRAssemblyPart>();
          if (part != null)
          {
            isOccupied = true;
              _attachedPart = part;
              _eventRadio.RaiseEvent(part.socketIDSO);
              OnPartSnapped?.Invoke();
          }
        }
        


        public bool IsOccupied()
        {
            return isOccupied;
        }

        public bool IsPartValid(BaseAssemblyPart part)
        {
            if (isOccupied) return false;
            if (part.socketIDSO != _typeIDSO) return false;
            if (_socketStepValidationSO != null && !_socketStepValidationSO.IsSocketAllowed(_typeIDSO)) return false;
            return true;
        }
    }
}
