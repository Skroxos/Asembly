using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarryComponent : MonoBehaviour
{
    public Vector3 HoldPosition = new(0, 0, 1.5f);
    public Quaternion HoldRotation = Quaternion.identity;
    [SerializeField] private PickUpRadio pickupRadioSO;
    [SerializeField] private DropRadio dropRadioSO;


    public bool IsPickedUp;
    private Collider _collider;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void OnPickedUp()
    {
        IsPickedUp = true;
        if (_rigidbody != null) _rigidbody.isKinematic = true;

        if (_collider != null) _collider.isTrigger = true;
        pickupRadioSO.RaiseEvent(GetComponent<AsemblyPart>());
    }

    public void OnDropped()
    {
        IsPickedUp = false;
        _rigidbody.isKinematic = false;
        _collider.isTrigger = false;
        dropRadioSO.RaiseEvent();
    }
}