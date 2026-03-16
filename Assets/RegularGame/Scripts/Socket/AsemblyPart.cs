using DG.Tweening;
using DroneAssembly.CarrySystem;
using UnityEngine;

namespace DroneAssembly.Socket
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CarryComponent))]
    public class AsemblyPart : MonoBehaviour
    {
        public SocketIDSO socketIDSO;
        private CarryComponent _carryComponent;
        private Collider _collider;
        private Rigidbody _rigidBody;
        public bool IsPickedUp => _carryComponent != null && _carryComponent.IsPickedUp;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _carryComponent = GetComponent<CarryComponent>();
        }

        public void AttachToSocket(Transform snapPoint)
        {
            if (_carryComponent != null && _carryComponent.IsPickedUp) return;
            _rigidBody.isKinematic = true;
            _collider.enabled = false;
            // could make a serialized field for the layer to set to, but for now just hardcoding it
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            transform.SetParent(snapPoint);
            transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
            transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
        }
    }
}