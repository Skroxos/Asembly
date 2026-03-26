// using System;
// using DroneAssembly.Radios;
// using DroneAssembly.Socket;
// using DroneAssembly.Validator;
// using DroneAssembly.VR_Port.Part;
// using UnityEngine;
//
// namespace DroneAssembly.VR_Port.Socket
// {
//     public class VRHybridSocket : MonoBehaviour, ISocketValidation, ISocketEvents
//     {
//         [SerializeField] private SocketIDSO typeID;
//         [SerializeField] private SocketStepValidationSO stepValidationSO;
//         [SerializeField] private EventRadio eventRadio;
//         
//         
//         private VRAssemblyPart _attachedPart;
//         private Transform _snapPoint;
//         private bool _isOccupied;
//         
//         public event Action OnPartSnapped;
//         public event Action OnPartExited;
//         public event Action<BaseAssemblyPart> OnValidPartEntered;
//         
//
//         private void Awake()
//         {
//             _snapPoint = gameObject.transform;
//         }
//
//         private void OnTriggerEnter(Collider other)
//         {
//             if (_isOccupied) return;
//
//             if (other.TryGetComponent(out VRAssemblyPart part))
//             {
//                 OnValidPartEntered?.Invoke(part);
//                 if (!part.IsPickedUp)
//                     if (TrySnapPart(part))
//                         SnapToSocket();
//             }
//         }
//
//         private void OnTriggerExit(Collider other)
//         {
//             if (_isOccupied) return;
//             if (_attachedPart != null && other.gameObject == _attachedPart.gameObject)
//             {
//                 _attachedPart = null;
//                 _isOccupied = false;
//             }
//
//             OnPartExited?.Invoke();
//         }
//
//         
//         private bool TrySnapPart(VRAssemblyPart part)
//         {
//             if (_isOccupied || part.socketIDSO != typeID || part.IsPickedUp) return false;
//
//             if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return false;
//
//             _isOccupied = true;
//             _attachedPart = part;
//             return true;
//         }
//
//         private void SnapToSocket()
//         {
//             _attachedPart.AttachToSocket(_snapPoint);
//             eventRadio.RaiseEvent(_attachedPart.socketIDSO);
//             OnPartSnapped?.Invoke();
//         }
//
//         public bool IsOccupied()
//         {
//             return _isOccupied;
//         }
//
//         public bool IsPartValid(BaseAssemblyPart part)
//         {
//             if (part.socketIDSO != typeID) return false;
//             if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return false;
//             return true;
//         }
//     }
// }
