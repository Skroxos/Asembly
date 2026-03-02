using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/SimpleEventRadio")]
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
