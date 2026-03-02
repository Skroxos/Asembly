using System;
using UnityEngine;

public class SimpleEventRadio : ScriptableObject
{
    public event Action OnRaised;
    public void RaiseEvent()
    {
        OnRaised?.Invoke();
    }
    private void OnDisable()
    {
        OnRaised = null;
    }
}