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
    public class VRSocketController : MonoBehaviour, IXRSelectFilter, ISocketValidation, IXRHoverFilter, ISocketEvents
    {
        [SerializeField] private SocketIDSO _typeIDSO;
        [SerializeField] private SocketStepValidationSO _socketStepValidationSO;
        [SerializeField] private EventRadio _eventRadio;
        public event Action OnPartSnapped;
        private XRSocketInteractor _socket;
        private VRAssemblyPart _attachedPart;
        
        
        private bool _isOccupied;
        
        private void Awake()
        {
            _socket = GetComponent<XRSocketInteractor>();
            _socket.selectFilters.Add(this);
            _socket.hoverFilters.Add(this);
        }

        private void OnEnable()
        {
            _socket.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            _socket.selectEntered.RemoveListener(OnSelectEntered);
        }
        // This is a bit of a hack to make sure that the socket can only be hovered if it can be selected, otherwise the hover visuals will show up even if the part can't be snapped in.
        public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
        {
            if (_isOccupied) return false;
            var part = interactable.transform.gameObject;
            if (!part.TryGetComponent(out VRAssemblyPart assemblyPart)) return false;
            if (assemblyPart.socketIDSO != _typeIDSO) return false;
            if (_socketStepValidationSO != null && !_socketStepValidationSO.IsSocketAllowed(_typeIDSO)) return false;
            return true;
        }
        
        // This is the main logic for determining if a part can be snapped into the socket. It checks if the socket is already occupied, if the part has the correct SocketIDSO, and if the step validation allows for this socket to be used.
        public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
        {
            var part = interactable.transform.gameObject;
            if (_attachedPart != null && part == _attachedPart.gameObject) return true;
            if (_isOccupied) return false;
            if (!part.TryGetComponent(out VRAssemblyPart assemblyPart)) return false;
            if (assemblyPart.socketIDSO != _typeIDSO) return false;
            if (_socketStepValidationSO != null && !_socketStepValidationSO.IsSocketAllowed(_typeIDSO)) return false;
            return true;
        }
        public bool canProcess => true;

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
          var part = args.interactableObject.transform.gameObject.GetComponent<VRAssemblyPart>();
          if (part != null)
          {
                _isOccupied = true;
              _attachedPart = part;
              var interactable = args.interactableObject as XRBaseInteractable;
              if (interactable != null)
              {
                  interactable.interactionLayers = InteractionLayerMask.GetMask("Snapped");
              }
              
              if (part.TryGetComponent<Rigidbody>(out var rb))
              {
                  rb.isKinematic = true;
              }
              
              if (part.TryGetComponent<Collider>(out var coll))
              {
                  coll.enabled = false;
              }
              
              OnPartSnapped?.Invoke();
              _eventRadio.RaiseEvent(part.socketIDSO);
          }
        }
        


        public bool IsOccupied()
        {
            return _isOccupied;
        }

        public bool IsPartValid(BaseAssemblyPart part)
        {
            if (_isOccupied) return false;
            if (part.socketIDSO != _typeIDSO) return false;
            if (_socketStepValidationSO != null && !_socketStepValidationSO.IsSocketAllowed(_typeIDSO)) return false;
            return true;
        }

    
    }
}
