using DroneAssembly.Player.Input;
using UnityEngine;

namespace DroneAssembly.Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private PlayerConfig playerConfig;
        private Vector3 _moveDirection;

        private float _xRotation;
        

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            HandleMovement();
        }

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

        private void HandleLook(Vector2 lookInput)
        {
            var mouseX = lookInput.x * playerConfig.MouseSensitivity * Time.deltaTime;
            var mouseY = lookInput.y * playerConfig.MouseSensitivity * Time.deltaTime;


            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -playerConfig.VerticalLookLimit, playerConfig.VerticalLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);


            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            var forward = playerCamera.transform.forward;
            var right = playerCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var moveDirection = forward * _moveDirection.z + right * _moveDirection.x;

            moveDirection.y = _moveDirection.y;

            controller.Move(moveDirection * (playerConfig.Speed * Time.deltaTime));
        }
        
    }
}