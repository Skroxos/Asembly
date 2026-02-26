using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, MyActions.ICoreActions, MyActions.IInteractionsActions, MyActions.IOtherActions
{
    public event Action<Vector3> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action RotateButtonStartedEvent;
    public event Action RotateButtonCanceledEvent;
    public event Action<Vector2> MouseDeltaEvent;
    public event Action<Vector2> MoveItemEvent;
   
    public event Action InteractEvent;
    
    
    private MyActions _inputActions;
    
    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new MyActions();
            _inputActions.Core.SetCallbacks(this);
            _inputActions.Interactions.SetCallbacks(this);
            _inputActions.Other.SetCallbacks(this);
        }
        
        _inputActions.Core.Enable();
        _inputActions.Interactions.Enable();
        _inputActions.Other.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Core.Disable();
        _inputActions.Interactions.Disable();
        _inputActions.Other.Disable();
    }
    
    private void EnableRotationMode()
    {
        _inputActions.Core.Disable();
        _inputActions.Interactions.Disable();
    }

    private void DisableRotationMode()
    {
        _inputActions.Core.Enable();
        _inputActions.Interactions.Enable();
    }
    
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector3>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractEvent?.Invoke();
        }
    }

    public void OnDistance(InputAction.CallbackContext context)
    {
        MoveItemEvent?.Invoke(context.ReadValue<Vector2>());
    }


    public void OnRotateButton(InputAction.CallbackContext context)
    {
       if (context.started)
       {
           RotateButtonStartedEvent?.Invoke();
           EnableRotationMode();
       }
       else if (context.canceled)
       {
           RotateButtonCanceledEvent?.Invoke();
           DisableRotationMode();
       }
    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        MouseDeltaEvent?.Invoke(context.ReadValue<Vector2>());
    }
}
