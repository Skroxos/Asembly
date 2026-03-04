using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarryComponent))]
public class AsemblyPart : MonoBehaviour
{
    public SocketIDSO socketIDSO;
    private CarryComponent carryComponent;
    private Collider collider;
    private Rigidbody rigidBody;
    public bool isPickedUp => carryComponent != null && carryComponent.IsPickedUp;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        carryComponent = GetComponent<CarryComponent>();
    }

    public void AttachToSocket(Transform snapPoint)
    {
        if (carryComponent != null && carryComponent.IsPickedUp) return;
        rigidBody.isKinematic = true;
        collider.enabled = false;

        transform.DOMove(snapPoint.position, 0.3f).SetEase(Ease.OutBack);
        transform.DORotateQuaternion(snapPoint.rotation, 0.3f);
        transform.SetParent(snapPoint);
    }
}