using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Inputs
{
    public class NonMobilePlayerControls : MonoBehaviour
    {
        private CharacterController characterController;
        private Camera mainCamera;
        private InputActions inputActions;
        [SerializeField]
        private RectTransform crossHair = default;
        private Vector2 movementInput;
        private Vector2 lookInput;
        private Vector3 lookHitPosition;
        [SerializeField]
        private float movementSpeed = 5;
        [SerializeField]
        private float rotationSpeed = 5;
        [SerializeField]
        private float minAngleFloatToRotate = 7.5f;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            inputActions = new InputActions();
            inputActions.Player.Movement.performed += MovementPerformed;
            inputActions.Player.Movement.canceled += MovementCanceled;
            inputActions.Player.Look.performed += LookPerformed;
        }

        private void OnEnable()
        {
            inputActions.Player.Movement.Enable();
            inputActions.Player.Look.Enable();
        }

        private void MovementPerformed(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        private void MovementCanceled(InputAction.CallbackContext context)
        {
            movementInput = Vector2.zero;
        }

        private void LookPerformed(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            Crosshair();
            Move();
            Rotation();
        }

        private void Crosshair()
        {
            crossHair.position = lookInput;
        }

        private void Move()
        {
            Vector3 moveDirectionSide = transform.right * movementInput.x;
            Vector3 moveDirectionForward = transform.forward * movementInput.y;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide) * Time.deltaTime;
            moveDirection = ApplyGravity(moveDirection);
            characterController.Move(moveDirection * movementSpeed);
        }

        private Vector3 ApplyGravity(Vector3 moveDirection)
        {
            if (!characterController.isGrounded)
            {
                moveDirection.y += Physics.gravity.y;
            }
            else
            {
                moveDirection.y = 0;
            }

            return moveDirection;
        }

        private void Rotation()
        {
            RotationInput();
            Vector3 relativePosition = (lookHitPosition - transform.position).normalized;
            Quaternion lookDirection = Quaternion.LookRotation(relativePosition, Vector3.up);
            float angleBetweenRotations = Quaternion.Angle(transform.rotation, lookDirection);
            if (angleBetweenRotations >= minAngleFloatToRotate)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.Euler(new Vector3(0, lookDirection.eulerAngles.y, 0)),
                    rotationSpeed * Time.deltaTime);
            }
        }

        private void RotationInput()
        {
            Ray screenRay = mainCamera.ScreenPointToRay(lookInput);
            if (Physics.Raycast(screenRay, out RaycastHit hit))
            {
                lookHitPosition = hit.point;
                lookHitPosition.y = 0;
            }
        }

        private void OnDisable()
        {
            inputActions.Player.Movement.Disable();
            inputActions.Player.Look.Disable();
        }
    }
}