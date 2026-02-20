using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/IteractionRadioSO")]
public class IteractionRadioSO : ScriptableObject
{
    public Action<AsemblyPart> OnPickUp;
    public Action OnDrop;
    
    public void RaisePickUp(AsemblyPart part)
    {
        OnPickUp?.Invoke(part);
    }
    
    public void RaiseDrop()
    {
        OnDrop?.Invoke();
    }
}