using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarryComponent : MonoBehaviour
{
    public Vector3 HoldPosition = new Vector3(0, 0, 1.5f);
    public Quaternion HoldRotation = Quaternion.identity;
    [SerializeField] private IteractionRadioSO iteractionRadioSO;
    
    private Rigidbody _rigidbody;
    private Collider _collider;
    

    public bool IsPickedUp;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void OnPickedUp()
    {
        IsPickedUp = true;
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
        }

        if (_collider != null)
        {
            _collider.isTrigger = true;
        }
        iteractionRadioSO.RaisePickUp(gameObject.GetComponent<AsemblyPart>());
    }

    public void OnDropped()
    {
        IsPickedUp = false;
        _rigidbody.isKinematic = false;
        _collider.isTrigger = false;
        iteractionRadioSO.RaiseDrop();
    }
}