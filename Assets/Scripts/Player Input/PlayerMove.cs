using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerMove : MonoBehaviour
    {
        private CharacterController characterController;
        private Vector2 deltaMoveValue;
        [SerializeField]
        private float moveSpeed = 0.3f;
        private bool joystickMove;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            InputEventHandler.JoystickMoveEvent += JoystickMove;
            InputEventHandler.JoystickMoveInputEvent += JoystickMoveInput;
        }

        private void JoystickMove(bool move)
        {
            // Method that controls whether or not moving should be activated.
            joystickMove = move;
        }

        private void JoystickMoveInput(Vector2 delta)
        {
            // The value from the joystick handling the movement.
            deltaMoveValue = delta;
        }

        private void Update()
        {
            if (joystickMove)
            {
                Move();
            }

            #if UNITY_STANDALONE
            if (characterController.velocity.sqrMagnitude > 0)
            {
                // "Temporary" solution to animation controller bugging due to velocity being over zero when being still.
                characterController.Move(Vector3.zero);
            }
            #endif
        }

        private void Move()
        {
            Vector3 moveDirection = CalculateDirection();
            moveDirection = ApplyGravity(moveDirection);
            characterController.Move(moveDirection * moveSpeed);
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

        private Vector3 CalculateDirection()
        {
            Vector3 moveDirectionForward = transform.forward * deltaMoveValue.y;
            Vector3 moveDirectionSide = transform.right * deltaMoveValue.x;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide) * Time.deltaTime;
            return moveDirection;
        }

        private void OnDisable()
        {
            InputEventHandler.JoystickMoveEvent -= JoystickMove;
            InputEventHandler.JoystickMoveInputEvent -= JoystickMoveInput;
        }
    }
}