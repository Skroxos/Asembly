using DroneAssembly.Radios;
using DroneAssembly.Radios.GeneralRadios;
using DroneAssembly.Socket;
using DroneAssembly.VR_Port.Part;
using UnityEngine;

namespace DroneAssembly.VR_Port.Socket
{
    [RequireComponent(typeof(VRSocketController))]
    public class VRSocketGhostVisual : MonoBehaviour
    {
        [SerializeField] private GhostPreviewConfig _ghostPreviewConfig;
        [SerializeField] private PickUpRadio _pickUpRadio;
        [SerializeField] private SimpleEventRadio _dropRadio;
        
        private VRSocketController _socketController;
        private GhostPreviewManager _ghostPreviewManager;
        private Transform _snapPoint;
        
        private void Awake()
        {
            _socketController = GetComponent<VRSocketController>();
            _ghostPreviewManager = new GhostPreviewManager(_ghostPreviewConfig.defaultMat, _ghostPreviewConfig.validMat);
            _snapPoint = gameObject.transform;
        }
        
        private void OnEnable()
        {
            _socketController.OnPartSnapped += HandlePartSnapped;
                _pickUpRadio.OnEventRaised += HandlePickUpPart;
                _dropRadio.OnRaised += HandleDropPart;
        }


        private void OnDisable()
        {
            _socketController.OnPartSnapped -= HandlePartSnapped;
            _pickUpRadio.OnEventRaised -= HandlePickUpPart;
            _dropRadio.OnRaised -= HandlePartSnapped;
        }
        
        private void HandleDropPart()
        {
            _ghostPreviewManager.HideGhost();
        }

        private void HandlePickUpPart(BaseAssemblyPart obj)
        {
          //  ShowGhostForPart(obj);
        }
        
        private void HandlePartSnapped()
        {
            _ghostPreviewManager.HideGhost();
        }
        
        // private void ShowGhostForPart(BaseAssemblyPart part)
        // {
        //     if (_socketController.IsOccupied) return;
        //     
        //     if (_socketController.IsPartValid(part))
        //     {
        //         _ghostPreviewManager.ShowGhost(part.gameObject, _snapPoint);
        //     }
        // }
    }
}
