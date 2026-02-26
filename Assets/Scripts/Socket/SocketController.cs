using System;
using UnityEngine;

public class SocketController : MonoBehaviour
{
    [SerializeField] private SocketIDSO typeID;
     private Transform snapPoint;
    [SerializeField] private SocketStepValidationSO stepValidationSO;
    [SerializeField] private IteractionRadioSO iteractionRadioSO;
    private AsemblyPart attachedPart;
    public bool IsOccupied;
    
    private GhostPreviewManager ghostManager;
    
    private GameObject ghostInstance;

    [SerializeField] private EventRadio eventRadio;

    private void OnEnable()
    {
      iteractionRadioSO.OnPickUp += HandlePartPickedUp;
        iteractionRadioSO.OnDrop += HandlePartDropped;
    }
    
    private void OnDisable()
    {
        iteractionRadioSO.OnPickUp -= HandlePartPickedUp;
        iteractionRadioSO.OnDrop -= HandlePartDropped;
    }
    

    private void HandlePartDropped()
    {
        ShowGhost(false);
    }

    private void HandlePartPickedUp(AsemblyPart obj)
    {
        if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return;
        if (obj.socketIDSO != typeID) return;
        if (IsOccupied) return;
        attachedPart = obj;
        ShowGhost(true);
    }

    private void Awake()
    {
        ghostManager = new GhostPreviewManager();
        snapPoint = gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOccupied) return;
            
        AsemblyPart part = other.GetComponent<AsemblyPart>();
        if (part != null && part.socketIDSO == typeID)
        {
            ghostManager.SetValidGhostMaterial();
            if (!part.isPickedUp)
            {
                bool snapped = TrySnapPart(part);
                if (snapped)
                {
                    SnapToSocket();
                }
            }


        }
          
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOccupied) return;
        ghostManager.DisableValidGhostMaterial();
        if (attachedPart != null && other.gameObject == attachedPart.gameObject)
        {
            attachedPart = null;
            IsOccupied = false;
        }
    }
    
    
    private bool TrySnapPart(AsemblyPart part)
    {
        if (IsOccupied || part.socketIDSO != typeID || part.isPickedUp) return false;
        
        if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID))
        {
            return false;
        }
        
        IsOccupied = true;
        attachedPart = part;
        return true;
    }
        
    private void SnapToSocket()
    {
        attachedPart.AttachToSocket(snapPoint);
        eventRadio.RaiseSnap(attachedPart.socketIDSO);
    }
    
    private void ShowGhost(bool show)
    {
        if (show)
        {
            ghostManager.ShowGhost(attachedPart.gameObject, snapPoint);
        }
        else
        {
            ghostManager.HideGhost();
        }
    }

}