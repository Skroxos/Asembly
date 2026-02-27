using System;
using UnityEngine;

[RequireComponent(typeof(SocketController))]
public class SocketGhostVisuals : MonoBehaviour
{
    [SerializeField] private IteractionRadioSO interactionRadioSO;
    private SocketController socketController;
    private GhostPreviewManager ghostPreviewManager;
    
    private Transform snapPoint;
    private AsemblyPart currentPart;

    private void Awake()
    {
        socketController = GetComponent<SocketController>();
        ghostPreviewManager = new GhostPreviewManager();
        snapPoint = transform;
    }

    private void OnEnable()
    {
        interactionRadioSO.OnPickUp += HandlePartPickedUp;
        interactionRadioSO.OnDrop += HandlePartDropped;
        
        socketController.OnPartSnapped += HandlePartSnapped;
        socketController.OnPartExited += HandlePartExited;
        socketController.OnValidPartEntered += HandleValidPartEntered;
    }

    private void OnDisable()
    {
        interactionRadioSO.OnPickUp -= HandlePartPickedUp;
        interactionRadioSO.OnDrop -= HandlePartDropped;
        
        socketController.OnPartSnapped -= HandlePartSnapped;
        socketController.OnPartExited -= HandlePartExited;
        socketController.OnValidPartEntered -= HandleValidPartEntered;
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

    private void HandlePartPickedUp(AsemblyPart obj)
    {
        if (socketController.IsOccupied) return;
        
        if (socketController.IsPartValid(obj))
        {
            currentPart = obj;
            ghostPreviewManager.ShowGhost(currentPart.gameObject, snapPoint);
        }
    }

}
