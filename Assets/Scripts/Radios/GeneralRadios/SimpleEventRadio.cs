using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/SimpleEventRadio")]
public class SimpleEventRadio : ScriptableObject
{
    private void OnDisable()
    {
        OnRaised = null;
    }

    public event Action OnRaised;

    public void RaiseEvent()
    {
        OnRaised?.Invoke();
    }
}