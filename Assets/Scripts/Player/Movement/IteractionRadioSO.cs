using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/IteractionRadioSO")]
public class IteractionRadioSO : ScriptableObject
{
    public Action<SocketIDSO> OnPickUp;
    public Action OnDrop;
    
    public void RaisePickUp(SocketIDSO socketID)
    {
        OnPickUp?.Invoke(socketID);
    }
    
    public void RaiseDrop()
    {
        OnDrop?.Invoke();
    }
}