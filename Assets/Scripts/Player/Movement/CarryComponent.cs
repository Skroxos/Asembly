using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class CarryComponent : MonoBehaviour
{
    public Vector3 HoldPosition = new Vector3(0, 0, 1.5f);
    public Quaternion HoldRotation = Quaternion.identity;
    
    private Rigidbody _rigidbody;
    private Collider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void OnPickedUp()
    {
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
        }

        if (_collider != null)
        {
            _collider.isTrigger = true;
        }
    }

    public void OnDropped()
    {
        _rigidbody.isKinematic = false;
        _collider.isTrigger = false;
    }
}