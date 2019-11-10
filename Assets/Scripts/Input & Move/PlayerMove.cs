using UnityEngine;

namespace Shooter.Input
{
    public class PlayerMove : MonoBehaviour
    {
        private CharacterController characterController;
        private Vector2 deltaInputValue;
        [SerializeField]
        private float moveSpeed = 0.3f;
        private bool move;
        private bool joystickMove;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            InputEventHandler.JoystickMoveEvent += JoystickMove;
            InputEventHandler.JoystickInputEvent += JoystickInput;
        }

        private void JoystickInput(Vector2 delta)
        {
            deltaInputValue = delta;
            move = true;
            if (Mathf.Approximately(delta.x, float.Epsilon)
                || Mathf.Approximately(delta.y, float.Epsilon))
            {
                move = false;
            }
        }

        private void JoystickMove(bool move)
        {
            joystickMove = move;
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
            Vector3 moveDirectionForward = transform.forward * deltaInputValue.y * Time.deltaTime;
            Vector3 moveDirectionSide = transform.right * deltaInputValue.x * Time.deltaTime;
            Vector3 moveDirection = moveDirectionForward + moveDirectionSide;
            return moveDirection;
        }

        private void OnDisable()
        {
            InputEventHandler.JoystickMoveEvent -= JoystickMove;
            InputEventHandler.JoystickInputEvent -= JoystickInput;
        }
    }
}