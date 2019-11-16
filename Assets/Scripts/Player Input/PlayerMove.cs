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
            joystickMove = move;
        }

        private void JoystickMoveInput(Vector2 delta)
        {
            deltaMoveValue = delta;
        }

        private void Update()
        {
            if (joystickMove)
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
            Vector3 moveDirectionForward = transform.forward * deltaMoveValue.y;
            Vector3 moveDirectionSide = transform.right * deltaMoveValue.x;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide) * Time.deltaTime;
            return moveDirection;
        }

        private void OnDestroy()
        {
            InputEventHandler.JoystickMoveEvent -= JoystickMove;
            InputEventHandler.JoystickMoveInputEvent -= JoystickMoveInput;
        }
    }
}