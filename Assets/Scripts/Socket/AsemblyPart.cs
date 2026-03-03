using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CarryComponent))]
public class AsemblyPart : MonoBehaviour
{
    public SocketIDSO socketIDSO;
    public bool isPickedUp => carryComponent != null && carryComponent.IsPickedUp;
    private Rigidbody rigidBody;
    private Collider collider;
    private CarryComponent carryComponent;
  
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