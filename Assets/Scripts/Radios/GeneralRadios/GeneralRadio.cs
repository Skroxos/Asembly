using System;
using UnityEngine;

public class GeneralRadio<T> : ScriptableObject
{
    public event Action<T> OnEventRaised;

    public void RaiseEvent(T value)
    {
        OnEventRaised?.Invoke(value);
    }

    private void OnDisable()
    {
        OnEventRaised = null;
    }
}