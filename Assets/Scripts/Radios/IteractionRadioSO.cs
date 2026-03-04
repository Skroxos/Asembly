using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/InteractionRadioSO")]
public class IteractionRadioSO : ScriptableObject
{
    public event Action<AsemblyPart> OnPickUp;
    public event Action OnDrop;

    public void RaisePickUp(AsemblyPart part)
    {
        OnPickUp?.Invoke(part);
    }

    public void RaiseDrop()
    {
        OnDrop?.Invoke();
    }
}