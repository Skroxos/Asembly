using System;
using UnityEngine;

public class SocketController : MonoBehaviour
{
    [SerializeField] private SocketIDSO typeID;
    [SerializeField] private Transform snapPoint;
    private AsemblyPart attachedPart;
    public bool IsOccupied;
    
    private GhostPreviewManager ghostManager;
    
    private GameObject ghostInstance;


    private void Awake()
    {
        ghostManager = new GhostPreviewManager();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOccupied) return;
            
        AsemblyPart part = other.GetComponent<AsemblyPart>();
        if (part != null && part.socketIDSO == typeID)
        {
            attachedPart = part;
            ShowGhost(true);
            TrySnapPart(part);
        }
          
    }

    private void OnTriggerExit(Collider other)
    {
        if (attachedPart != null && other.gameObject == attachedPart.gameObject)
        {
            ShowGhost(false);
            attachedPart = null;
            IsOccupied = false;
        }
    }
    
    
    private bool TrySnapPart(AsemblyPart part)
    {
        if (IsOccupied || part.socketIDSO != typeID || part.isPickedUp) return false;
            
        attachedPart = part;
        SnapToSocket();
        ShowGhost(false);
        return true;
    }
        
    private void SnapToSocket()
    {
        attachedPart.AttachToSocket(snapPoint);
        IsOccupied = true;
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