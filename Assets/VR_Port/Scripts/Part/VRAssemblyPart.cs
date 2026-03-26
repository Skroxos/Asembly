// using DG.Tweening;
// using DroneAssembly.Socket;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace DroneAssembly.VR_Port.Part
{
    public class VRAssemblyPart : BaseAssemblyPart
    {
     // private XRGrabInteractable _grabInteractable;
     // private Rigidbody _rigidBody;
     // private VRPickUpRadioListener _pickUpRadioListener;
     // public bool IsPickedUp => _grabInteractable.isSelected;
     //
     //    private void Awake()
     //    {
     //        _grabInteractable = GetComponent<XRGrabInteractable>();
     //            _rigidBody = GetComponent<Rigidbody>();
     //                _pickUpRadioListener = GetComponent<VRPickUpRadioListener>();
     //    }
     //
     //
     //    public void AttachToSocket(Transform snapPoint)
     //    {
     //        if (IsPickedUp) return;
     //        RemoveComponentsAfterSnap();
     //        gameObject.layer = LayerMask.NameToLayer("Snapped Part");
     //        transform.SetParent(snapPoint);
     //        transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
     //        transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
     //    }   
     //
     //    private void RemoveComponentsAfterSnap()
     //    {
     //        Destroy(_pickUpRadioListener);
     //        Destroy(_grabInteractable);
     //        Destroy(_rigidBody);
     //    }
    }
}

