using System;
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
        private ISocketValidation socketValidation;

        private void Awake()
        {
            socketValidation = GetComponent<ISocketValidation>();
            socketEvents = GetComponent<ISocketEvents>();
            ghostPreviewManager = new GhostPreviewManager(ghostPreviewConfig.defaultMat, ghostPreviewConfig.validMat);
            snapPoint = transform;
        }

        private void Update()
        {
            ghostPreviewManager.UpdateGhostPosition(snapPoint);
        }

        private void OnEnable()
        {
         
            socketEvents.OnPartExited += HandlePartExited;
            socketEvents.OnValidPartEntered += HandleValidPartEntered;
            socketEvents.OnPartSnapped += HandlePartSnapped;
            pickUpRadioSO.OnEventRaised += HandlePartPickedUp;
            dropRadioSO.OnRaised += HandlePartDropped;
        }
        

        private void OnDisable()
        {
            pickUpRadioSO.OnEventRaised -= HandlePartPickedUp;
            dropRadioSO.OnRaised -= HandlePartDropped;
            socketEvents.OnPartSnapped -= HandlePartSnapped;
            socketEvents.OnValidPartEntered -= HandleValidPartEntered;
            socketEvents.OnPartExited -= HandlePartExited;
        }

        private void HandleValidPartEntered(BaseAssemblyPart obj)
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