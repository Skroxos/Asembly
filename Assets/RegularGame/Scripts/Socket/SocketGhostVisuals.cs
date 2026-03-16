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

        private Transform snapPoint;
        private SocketController socketController;
        private ISocketValidation socketValidation;

        private void Awake()
        {
            socketController = GetComponent<SocketController>();
            socketValidation = GetComponent<ISocketValidation>();
            ghostPreviewManager = new GhostPreviewManager(ghostPreviewConfig.defaultMat, ghostPreviewConfig.validMat);
            snapPoint = transform;
        }

        private void OnEnable()
        {
            if (socketController != null)
            {
                socketController.OnPartSnapped += HandlePartSnapped;
                socketController.OnPartExited += HandlePartExited;
                socketController.OnValidPartEntered += HandleValidPartEntered;
            }
            pickUpRadioSO.OnEventRaised += HandlePartPickedUp;
            dropRadioSO.OnRaised += HandlePartDropped;
        }
        

        private void OnDisable()
        {
            pickUpRadioSO.OnEventRaised -= HandlePartPickedUp;
            dropRadioSO.OnRaised -= HandlePartDropped;
            if (socketController != null)
            {
                socketController.OnPartSnapped -= HandlePartSnapped;
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