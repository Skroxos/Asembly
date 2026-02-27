using UnityEngine;
using System;

public class SocketController : MonoBehaviour
{
    [SerializeField] private SocketIDSO typeID;
     public bool IsOccupied { get; private set; }
     private Transform snapPoint;
     
     public event Action OnPartSnapped;
     public event Action OnPartExited;
     public event Action<AsemblyPart> OnValidPartEntered;
     
     
    [SerializeField] private SocketStepValidationSO stepValidationSO;
    [SerializeField] private EventRadio eventRadio;
    
    private AsemblyPart attachedPart;
    private GameObject ghostInstance;

    

    private void Awake()
    {
        snapPoint = gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOccupied) return;

        if (other.TryGetComponent(out AsemblyPart part))
        {
            if (part.socketIDSO != typeID ) return;
            OnValidPartEntered?.Invoke(part);
            if (!part.isPickedUp)
            {
                if (TrySnapPart(part))
                {
                    SnapToSocket();
                }
            }
            
        }
          
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOccupied) return;
            OnPartExited?.Invoke();
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
        OnPartSnapped?.Invoke();
    }
    
    public bool IsPartValid(AsemblyPart part)
    {
        if (part.socketIDSO != typeID) return false;
        if (stepValidationSO != null && !stepValidationSO.IsSocketAllowed(typeID)) return false;
        return true;
    }
}