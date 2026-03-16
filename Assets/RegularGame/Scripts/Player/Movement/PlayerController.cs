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
        private float _originalCameraHeight;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _originalCameraHeight = playerCamera.transform.localPosition.y;
        }

        private void Update()
        {
            HandleMovement();
        }

        private void OnEnable()
        {
            inputReader.MoveEvent += OnMove;
            inputReader.LookEvent += OnLook;
            inputReader.CrouchEvent += OnCrouch;
        }


        private void OnDisable()
        {
            inputReader.MoveEvent -= OnMove;
            inputReader.LookEvent -= OnLook;
            inputReader.CrouchEvent -= OnCrouch;
        }
        private void OnCrouch(bool isCrouching)
        {
            if (isCrouching)
            {
                PlayerHeightChange(playerConfig.CrouchHeight);
                
            }
            else
            {
                PlayerHeightChange(playerConfig.StandingHeight);
            }
        }
        

        private void OnLook(Vector2 obj)
        {
            HandleLook(obj);
        }

        private void OnMove(Vector3 obj)
        {
            _moveDirection = obj;
        }
        private void PlayerHeightChange(float targetHeight)
        {
            controller.height = targetHeight;

            Vector3 center = controller.center;
            center.y = controller.height / 2f;
            controller.center = center;

            Vector3 cameraPosition = playerCamera.transform.localPosition;
            cameraPosition.y = _originalCameraHeight - (playerConfig.StandingHeight - targetHeight);
            playerCamera.transform.localPosition = cameraPosition;
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