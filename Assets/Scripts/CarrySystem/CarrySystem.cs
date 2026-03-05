using UnityEngine;

public class CarrySystem : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private AudioClipRadio audioClipRadio;
    [SerializeField] private AudioConfig pickUpSound;
    private bool _allowRotation;
    private CarryComponent _carriedObject;

    // could be in a config scriptable object if we want to be able to change it in runtime
    [SerializeField] private float distanceStep = 0.25f;
    [SerializeField] private float minHoldDistance;
    [SerializeField] private float maxHoldDistance;
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float pickUpDistance = 3f;
    private void OnEnable()
    {
        inputReader.InteractEvent += HandleInteract;
        inputReader.MoveItemEvent += HandleMoveItem;
        inputReader.RotateButtonStartedEvent += HandleRotateStarted;
        inputReader.RotateButtonCanceledEvent += HandleRotateCanceled;
        inputReader.MouseDeltaEvent += HandleRotation;
    }


    public void OnDisable()
    {
        inputReader.InteractEvent -= HandleInteract;
        inputReader.RotateButtonStartedEvent -= HandleRotateStarted;
        inputReader.RotateButtonCanceledEvent -= HandleRotateCanceled;
        inputReader.MouseDeltaEvent -= HandleRotation;
        inputReader.MoveItemEvent -= HandleMoveItem;
    }

    private void HandleRotateCanceled()
    {
        _allowRotation = false;
    }

    private void HandleRotateStarted()
    {
        _allowRotation = true;
    }

    private void HandleRotation(Vector2 obj)
    {
        if (_carriedObject == null || !_allowRotation) return;
        var rotationInput = obj;
        
        var mouseX = rotationInput.x * rotationSpeed;
        var mouseY = rotationInput.y * rotationSpeed;


        _carriedObject.transform.Rotate(playerCamera.transform.up, -mouseX, Space.World);
        _carriedObject.transform.Rotate(playerCamera.transform.right, mouseY, Space.World);
    }

    private void HandleMoveItem(Vector2 obj)
    {
        MoveCarriedObject(obj);
    }

    private void HandleInteract()
    {
        if (_carriedObject == null)
            TryToPickUp();
        else
            Drop();
    }

    private void MoveCarriedObject(Vector2 input)
    {
        if (_carriedObject == null) return;

        var newPosition = holdPoint.localPosition;
        newPosition.z += input.y * distanceStep;

        newPosition.z = Mathf.Clamp(newPosition.z, minHoldDistance, maxHoldDistance);

        holdPoint.localPosition = newPosition;
    }

    private void TryToPickUp()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hit, pickUpDistance))
            if (hit.collider.TryGetComponent(out CarryComponent carryComponent))
                PickUp(carryComponent);
    }

    private void PickUp(CarryComponent carryComponent)
    {
        if (pickUpSound != null && pickUpSound.audioClips.Length > 0) audioClipRadio.RaiseEvent(pickUpSound);
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