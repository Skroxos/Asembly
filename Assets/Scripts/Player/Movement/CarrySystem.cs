using UnityEngine;

public class CarrySystem : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float distanceStep = 0.5f;
    [SerializeField] private InputReader inputReader;
    private CarryComponent _carriedObject;

    private void OnEnable()
    {
        inputReader.InteractEvent += HandleInteract;
        inputReader.MoveItemEvent += HandleMoveItem;
    }
    public void OnDisable()
    {
        inputReader.InteractEvent -= HandleInteract;
        inputReader.MoveItemEvent -= HandleMoveItem;
    }

    private void HandleMoveItem(Vector2 obj)
    {
        MoveCarriedObject(obj);
    }

    private void HandleInteract()
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

    private void MoveCarriedObject(Vector2 input)
    {
        if (_carriedObject == null) return;

        Vector3 newPosition = holdPoint.localPosition;
        newPosition.z += input.y * distanceStep;
        
        newPosition.z = Mathf.Clamp(newPosition.z, 0.5f, 5f); 

        holdPoint.localPosition = newPosition;
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