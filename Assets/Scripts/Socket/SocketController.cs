using UnityEngine;

public class SocketController : MonoBehaviour
{
    [SerializeField] private SocketIDSO typeID;
    [SerializeField] private Transform snapPoint;
    private AsemblyPart attachedPart;
    public bool IsOccupied;
    
    private GhostPreviewManager ghostManager;
    
    private GameObject ghostInstance;

    [SerializeField] private EventRadio eventRadio;
    
    private void Awake()
    {
        ghostManager = new GhostPreviewManager();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOccupied) return;
            
        AsemblyPart part = other.GetComponent<AsemblyPart>();
        if (part != null && part.socketIDSO == typeID && !part.isPickedUp)
        {
            bool snapped = TrySnapPart(part);
            if (snapped)
            {
                SnapToSocket();
            }
           
           
        }
          
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOccupied) return;
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
            
        IsOccupied = true;
        attachedPart = part;
        ShowGhost(false);
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