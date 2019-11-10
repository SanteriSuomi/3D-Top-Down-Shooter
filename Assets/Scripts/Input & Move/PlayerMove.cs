using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Input
{
    public class PlayerMove : MonoBehaviour
    {
        private CharacterController characterController;
        private InputActions inputActions;
        private Vector2 inputValue;
        [SerializeField]
        private float moveSpeed = 0.3f;
        private bool move;
        private bool joystickMove;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            inputActions = new InputActions();
            inputActions.Player.Move.performed += MovePerformed;
            inputActions.Player.Move.canceled += MoveCanceled;
            InputEventHandler.JoystickMoveEvent += JoystickMove;
            InputEventHandler.JoystickStopEvent += JoystickStop;
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
        }

        private void MovePerformed(InputAction.CallbackContext context)
        {
            inputValue = context.ReadValue<Vector2>();
            move = true;
        }

        private void MoveCanceled(InputAction.CallbackContext context)
        {
            move = false;
        }

        private void JoystickMove()
        {
            joystickMove = true;
        }

        private void JoystickStop()
        {
            joystickMove = false;
        }

        private void Update()
        {
            if (move && joystickMove)
            {
                Move();
            }
        }

        private void Move()
        {
            Vector3 moveDirection = CalculateDirection();
            characterController.Move(moveDirection * moveSpeed);
        }

        private Vector3 CalculateDirection()
        {
            Vector3 moveDirectionForward = transform.forward * inputValue.y * Time.deltaTime;
            Vector3 moveDirectionSide = transform.right * inputValue.x * Time.deltaTime;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide).normalized;
            return moveDirection;
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
            InputEventHandler.JoystickMoveEvent -= JoystickMove;
            InputEventHandler.JoystickStopEvent -= JoystickStop;
        }
    }
}