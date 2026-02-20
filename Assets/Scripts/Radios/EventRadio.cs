using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/SocketSnap")]
public class EventRadio : ScriptableObject
{
    public event Action<SocketIDSO> OnSnap;
    
    public void RaiseSnap(SocketIDSO id)
    {
        OnSnap?.Invoke(id);
    }

    private void OnDisable()
    {
        OnSnap = null;
    }
}