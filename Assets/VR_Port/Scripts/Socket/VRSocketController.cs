using System;
using DG.Tweening;
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
        [SerializeField] private InteractionLayerMask _snappedLayerMask;
        public event Action OnPartSnapped;
        public event Action OnPartExited;
        public event Action<BaseAssemblyPart> OnValidPartEntered;
        
        private XRSocketInteractor _socket;
        private VRAssemblyPart _attachedPart;
        
        
        private bool _isOccupied;
        public bool canProcess => true;
        
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
            return ValidatePart(interactable.transform.gameObject);
        }
        
        // This is the main logic for determining if a part can be snapped into the socket. It checks if the socket is already occupied, if the part has the correct SocketIDSO, and if the step validation allows for this socket to be used.
        public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
        {
            var part = interactable.transform.gameObject;
            if (_attachedPart != null && part == _attachedPart.gameObject) return true;
            return ValidatePart(part);
        }
        
        private bool ValidatePart(GameObject partObject)
        {
            if (_isOccupied) return false;
            if (!partObject.TryGetComponent(out VRAssemblyPart assemblyPart)) return false;
            if (assemblyPart.socketIDSO != _typeIDSO) return false;
            if (_socketStepValidationSO != null && !_socketStepValidationSO.IsSocketAllowed(_typeIDSO)) return false;
            return true;
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (!args.interactableObject.transform.TryGetComponent(out VRAssemblyPart part))
            {
                Debug.LogWarning($"[{name}] SelectEntered fired but no VRAssemblyPart found.", this);
                return;
            }

            _isOccupied = true;
            _attachedPart = part;

            LockPartInteraction(part, args);

            OnPartSnapped?.Invoke();

            if (_eventRadio != null)
                _eventRadio.RaiseEvent(part.socketIDSO);
            else
                Debug.LogError($"[{name}] EventRadio is not assigned!", this);
        }
        
        
        private void LockPartInteraction(VRAssemblyPart part, SelectEnterEventArgs args)
        {
            Transform attachPoint = _socket.attachTransform != null ? _socket.attachTransform : transform;
            // var rootGrab = gameObject.GetComponentInParent<XRGrabInteractable>();
            // var collider = part.GetComponent<Collider>();
            part.transform.position = attachPoint.position;
            part.transform.rotation = attachPoint.rotation;
            part.transform.SetParent(this.transform, true);
            part.gameObject.layer = LayerMask.NameToLayer("Snapped Part"); 
            RemoveComponentsAfterSnap(part);
            // if (!rootGrab.colliders.Contains(collider))
            // {
            //     rootGrab.colliders.Add(collider);
            // }
        }
        
        
        private void RemoveComponentsAfterSnap(VRAssemblyPart part)
        {
            if (part.TryGetComponent<VRPickUpRadioListener>(out var radioListener))
            {
                Destroy(radioListener);
            }
            
            if (part.TryGetComponent<XRGrabInteractable>(out var oldGrab))
            {
                Destroy(oldGrab);
            }


            if (part.TryGetComponent<Rigidbody>(out var rb))
            {
                Destroy(rb);
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
