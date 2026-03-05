using System;
using UnityEngine;

namespace DroneAssembly.Radios.GeneralRadios
{
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
}