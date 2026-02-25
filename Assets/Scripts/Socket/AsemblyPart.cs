using UnityEngine;

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
            
        transform.position = snapPoint.position;
        transform.rotation = snapPoint.rotation;
        transform.SetParent(snapPoint);
    }
}