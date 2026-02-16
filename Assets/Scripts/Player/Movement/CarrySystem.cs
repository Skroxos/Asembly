using UnityEngine;

public class CarrySystem : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Camera playerCamera;
    
    private CarryComponent _carriedObject;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_carriedObject == null)
            {
                TryToPickUp();
            }
            else
            {
                Drop();
            }
        }
    }
    
    private void TryToPickUp()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, 3f))
        {
            if (hit.collider.TryGetComponent(out CarryComponent carryComponent))
            {
                PickUp(carryComponent);
            }
        }
    }
    
    private void PickUp(CarryComponent carryComponent)
    {
        _carriedObject = carryComponent;
        _carriedObject.OnPickedUp();
        _carriedObject.transform.SetParent(holdPoint);
        _carriedObject.transform.localPosition = _carriedObject.HoldPosition;
        _carriedObject.transform.localRotation = _carriedObject.HoldRotation;
    }
    
    private void Drop()
    {
        _carriedObject.OnDropped();
        _carriedObject.transform.SetParent(null);
        _carriedObject = null;
    }
}