using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Inputs
{
    public class NonMobilePlayerControls : MonoBehaviour
    {
        //
        // NonMobilePlayerControls is the controls (rotation/movement) for non-mobile platforms such as PC.
        //
        [SerializeField]
        private RectTransform crossHair = default;
        private CharacterController characterController;
        private Camera mainCamera;
        private InputActions inputActions;
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
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            // Start observing all the needed events from the input system.
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
            // Read the movement value from the input system.
            movementInput = context.ReadValue<Vector2>();
        }

        private void MovementCanceled(InputAction.CallbackContext context)
        {
            // When the input gets canceled, reset the movement variable.
            movementInput = Vector2.zero;
        }

        private void LookPerformed(InputAction.CallbackContext context)
        {
            // Read the look value from the input system
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
            Vector3 moveDirection = CalculateDirection();
            characterController.Move(moveDirection * movementSpeed);
        }

        private Vector3 CalculateDirection()
        {
            // movementDirectionSide and Forward are their respective movement direction's + the input value.
            Vector3 moveDirectionSide = transform.right * movementInput.x;
            Vector3 moveDirectionForward = transform.forward * movementInput.y;
            // Create a movementDirection from the two movementDirections, and make sure it's framerate independent.
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide) * Time.deltaTime;
            return moveDirection;
        }

        private void Rotation()
        {
            RotationInput();
            // Relative position is the direction vector from player to the target (mouse position).
            Vector3 relativePosition = (lookHitPosition - transform.position).normalized;
            // Create a lookDirection from the relative position.
            Quaternion lookDirection = Quaternion.LookRotation(relativePosition, Vector3.up);
            // Calculate the angle from current rotation to  the lookRotation (lookDirection).
            float angleBetweenRotations = Quaternion.Angle(transform.rotation, lookDirection);
            // Only rotate when angle is greate than X, so it's not too sensitive.
            if (angleBetweenRotations >= minAngleFloatToRotate)
            {
                // Spherically interpolate the rotation to the wanted rotation.
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.Euler(new Vector3(0, lookDirection.eulerAngles.y, 0)),
                    rotationSpeed * Time.deltaTime);
            }
        }

        private void RotationInput()
        {
            // Cast a ray from camera to the ground.
            Ray screenRay = mainCamera.ScreenPointToRay(lookInput);
            if (Physics.Raycast(screenRay, out RaycastHit hit))
            {
                // Store the position of the ray point that hit the ground.
                lookHitPosition = hit.point;
                // Reset lookHitPosition since we don't need/want it.
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