using System;
using DroneAssembly.Radios;
using DroneAssembly.Validator;
using UnityEngine;

namespace DroneAssembly.Socket
{
    public class SocketController : MonoBehaviour
    {
        [SerializeField] private SocketIDSO typeID;


        [SerializeField] private SocketStepValidationSO stepValidationSO;
        [SerializeField] private EventRadio eventRadio;

        private AsemblyPart _attachedPart;
        private GameObject _ghostInstance;
        private Transform _snapPoint;
        public bool IsOccupied { get; private set; }


        private void Awake()
        {
            _snapPoint = gameObject.transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsOccupied) return;

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
            if (IsOccupied) return;
            if (_attachedPart != null && other.gameObject == _attachedPart.gameObject)
            {
                _attachedPart = null;
                IsOccupied = false;
            }

            OnPartExited?.Invoke();
        }

        public event Action OnPartSnapped;
        public event Action OnPartExited;
        public event Action<AsemblyPart> OnValidPartEntered;


        private bool TrySnapPart(AsemblyPart part)
        {
            if (IsOccupied || part.socketIDSO != typeID || part.IsPickedUp) return false;

            if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return false;

            IsOccupied = true;
            _attachedPart = part;
            return true;
        }

        private void SnapToSocket()
        {
            _attachedPart.AttachToSocket(_snapPoint);
            eventRadio.RaiseEvent(_attachedPart.socketIDSO);
            OnPartSnapped?.Invoke();
        }

        public bool IsPartValid(AsemblyPart part)
        {
            if (part.socketIDSO != typeID) return false;
            if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return false;
            return true;
        }
    }
}