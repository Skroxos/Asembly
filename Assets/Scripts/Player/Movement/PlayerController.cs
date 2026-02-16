using UnityEngine;

namespace Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform playerCamera;

        [Header("Movement Settings")]
        [SerializeField] private float speed = 6.0f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 1.0f;

        [Header("Mouse Settings")]
        [SerializeField] private float mouseSensitivity = 100f;

        private Vector3 _velocity;
        private float _xRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            if (playerCamera == null && Camera.main != null)
            {
                playerCamera = Camera.main.transform;
            }
        }

        private void Update()
        {
            HandleLook();
            HandleMovement();
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            
            if (playerCamera != null)
            {
                playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            }
            
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            if (controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(speed * Time.deltaTime * move);
            
            _velocity.y += gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
        }
    }
}
