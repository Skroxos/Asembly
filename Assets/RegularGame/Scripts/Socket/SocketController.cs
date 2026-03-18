using System;
using DroneAssembly.Radios;
using DroneAssembly.Validator;
using UnityEngine;

namespace DroneAssembly.Socket
{
    public class SocketController : MonoBehaviour, ISocketValidation, ISocketEvents
    {
        [SerializeField] private SocketIDSO typeID;
        [SerializeField] private SocketStepValidationSO stepValidationSO;
        [SerializeField] private EventRadio eventRadio;

        private AsemblyPart _attachedPart;
        private GameObject _ghostInstance;
        private Transform _snapPoint;
        private bool _isOccupied;
        
        public event Action OnPartSnapped;
        public event Action OnPartExited;
        public event Action<AsemblyPart> OnValidPartEntered;

        private void Awake()
        {
            _snapPoint = gameObject.transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isOccupied) return;

            if (other.TryGetComponent(out AsemblyPart part))
            {
                OnValidPartEntered?.Invoke(part);
                if (!part.IsPickedUp)
                    if (TrySnapPart(part))
                        SnapToSocket();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isOccupied) return;
            if (_attachedPart != null && other.gameObject == _attachedPart.gameObject)
            {
                _attachedPart = null;
                _isOccupied = false;
            }

            OnPartExited?.Invoke();
        }

        
        private bool TrySnapPart(AsemblyPart part)
        {
            if (_isOccupied || part.socketIDSO != typeID || part.IsPickedUp) return false;

            if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return false;

            _isOccupied = true;
            _attachedPart = part;
            return true;
        }

        private void SnapToSocket()
        {
            _attachedPart.AttachToSocket(_snapPoint);
            eventRadio.RaiseEvent(_attachedPart.socketIDSO);
            OnPartSnapped?.Invoke();
        }

        public bool IsOccupied()
        {
            return _isOccupied;
        }

        public bool IsPartValid(BaseAssemblyPart part)
        {
            if (part.socketIDSO != typeID) return false;
            if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return false;
            return true;
        }

       
    }
}