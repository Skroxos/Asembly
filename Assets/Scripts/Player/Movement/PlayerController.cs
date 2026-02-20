using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float verticalLookLimit = 80f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float mouseSensitivity = 360f;
        
        private float _xRotation;
        private Vector3 _moveDirection;

        private void OnEnable()
        {
            inputReader.MoveEvent += OnMove;
            inputReader.LookEvent += OnLook;
        }

        private void OnDisable()
        {
            inputReader.MoveEvent -= OnMove;
            inputReader.LookEvent -= OnLook;
        }
        private void OnLook(Vector2 obj)
        {
            HandleLook(obj);
        }

        private void OnMove(Vector3 obj)
        {
            _moveDirection = obj;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleLook(Vector2 lookInput)
        {
            float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

       
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -verticalLookLimit, verticalLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            Vector3 forward = playerCamera.transform.forward;
            Vector3 right = playerCamera.transform.right;
        
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
        
            Vector3 moveDirection = (forward * _moveDirection.z + right * _moveDirection.x);
        
            moveDirection.y = _moveDirection.y;
        
            controller.Move(moveDirection * speed * Time.deltaTime);
        }


        private void TryToInteract()
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 3f))
            {
                if (hit.collider.TryGetComponent(out IInteractible interactible))
                {
                    interactible.Interact();
                }
            }
        }
    }
}