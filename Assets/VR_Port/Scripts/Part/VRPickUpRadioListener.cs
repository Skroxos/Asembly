using DroneAssembly.Radios;
using DroneAssembly.Radios.GeneralRadios;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace DroneAssembly.VR_Port.Part
{
    [RequireComponent(typeof(VRAssemblyPart))]
    [RequireComponent(typeof(XRGrabInteractable))]
    public class VRPickUpRadioListener : MonoBehaviour
    {
        [SerializeField] private PickUpRadio _pickUpRadio;
        [SerializeField] private SimpleEventRadio _dropRadio;
    
        private XRGrabInteractable _grabInteractable;
        private VRAssemblyPart _assemblyPart;
        
        
        private void Awake()
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
            _assemblyPart = GetComponent<VRAssemblyPart>();
        }
    
        private void OnEnable()
        {
            _grabInteractable.selectEntered.AddListener(OnPickUp);
            _grabInteractable.selectExited.AddListener(OnDrop);
        }


        private void OnDisable()
        {
            _grabInteractable.selectEntered.RemoveListener(OnPickUp);
            _grabInteractable.selectExited.RemoveListener(OnDrop);
        }
    
        private void OnDrop(SelectExitEventArgs arg0)
        {
            if (arg0.interactorObject is XRSocketInteractor) return;
            gameObject.layer = LayerMask.NameToLayer("Loose Part");
            _dropRadio.RaiseEvent();
        }

        private void OnPickUp(SelectEnterEventArgs arg0)
        {
            if (arg0.interactorObject is XRSocketInteractor) return;
            gameObject.layer = LayerMask.NameToLayer("Held Part");
            _pickUpRadio.RaiseEvent(_assemblyPart);
        }
    }
}
