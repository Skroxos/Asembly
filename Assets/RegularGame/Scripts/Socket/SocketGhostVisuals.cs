using DroneAssembly.Radios;
using DroneAssembly.Radios.GeneralRadios;
using UnityEngine;

namespace DroneAssembly.Socket
{
    public class SocketGhostVisuals : MonoBehaviour
    {
        [SerializeField] private PickUpRadio pickUpRadioSO;
        [SerializeField] private SimpleEventRadio dropRadioSO;
        [SerializeField] private GhostPreviewConfig ghostPreviewConfig;
        
        
        private BaseAssemblyPart currentPart;
        private GhostPreviewManager ghostPreviewManager;
        private ISocketEvents socketEvents;
        private Transform snapPoint;
        private SocketController socketController;
        private ISocketValidation socketValidation;

        private void Awake()
        {
            socketController = GetComponent<SocketController>();
            socketValidation = GetComponent<ISocketValidation>();
            socketEvents = GetComponent<ISocketEvents>();
            ghostPreviewManager = new GhostPreviewManager(ghostPreviewConfig.defaultMat, ghostPreviewConfig.validMat);
            snapPoint = transform;
        }

        private void OnEnable()
        {
            if (socketController != null)
            {
                socketController.OnPartExited += HandlePartExited;
                socketController.OnValidPartEntered += HandleValidPartEntered;
            }
            socketEvents.OnPartSnapped += HandlePartSnapped;
            pickUpRadioSO.OnEventRaised += HandlePartPickedUp;
            dropRadioSO.OnRaised += HandlePartDropped;
        }
        

        private void OnDisable()
        {
            pickUpRadioSO.OnEventRaised -= HandlePartPickedUp;
            dropRadioSO.OnRaised -= HandlePartDropped;
            socketEvents.OnPartSnapped -= HandlePartSnapped;
            if (socketController != null)
            {
                socketController.OnPartExited -= HandlePartExited;
                socketController.OnValidPartEntered -= HandleValidPartEntered;
            }
        }

        private void HandleValidPartEntered(AsemblyPart obj)
        {
            ghostPreviewManager.SetValidGhostMaterial();
        }

        private void HandlePartExited()
        {
            ghostPreviewManager.DisableValidGhostMaterial();
        }

        private void HandlePartSnapped()
        {
            currentPart = null;
            ghostPreviewManager.HideGhost();
        }

        private void HandlePartDropped()
        {
            currentPart = null;
            ghostPreviewManager.HideGhost();
            ghostPreviewManager.DisableValidGhostMaterial();
        }

        private void HandlePartPickedUp(BaseAssemblyPart obj)
        {
            if (socketValidation.IsOccupied()) return;

            if (socketValidation.IsPartValid(obj))
            {
                currentPart = obj;
                ghostPreviewManager.ShowGhost(currentPart.gameObject, snapPoint);
            }
        }
    }
}