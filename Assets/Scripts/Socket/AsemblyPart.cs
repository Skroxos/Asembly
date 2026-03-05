using DG.Tweening;
using UnityEngine;

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
        transform.SetParent(snapPoint);
        transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
        transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
    }
}