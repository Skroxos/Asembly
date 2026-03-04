using System;
using UnityEngine;

public class GeneralRadio<T> : ScriptableObject
{
    private void OnDisable()
    {
        OnEventRaised = null;
    }

    public event Action<T> OnEventRaised;

    public void RaiseEvent(T value)
    {
        OnEventRaised?.Invoke(value);
    }
}