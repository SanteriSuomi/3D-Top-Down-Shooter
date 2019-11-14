using Shooter.Player;
using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerMove : MonoBehaviour
    {
        private CharacterController characterController;
        private PlayerSettings playerSettings;
        private Vector2 deltaMoveValue;
        [SerializeField]
        private float moveSpeed = 0.3f;
        private bool move;
        private bool joystickMove;

        private void Awake()
        {
            playerSettings = GetComponent<PlayerSettings>();
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
            move = true;
            if (Mathf.Approximately(delta.x, float.Epsilon)
                || Mathf.Approximately(delta.y, float.Epsilon))
            {
                move = false;
            }
        }

        private void Update()
        {
            if (joystickMove && move)
            {
                Move();
            }
        }

        private void Move()
        {
            Vector3 moveDirection = CalculateDirection();
            characterController.Move(moveDirection * moveSpeed * playerSettings.PlayerSensitivityMultiplier);
        }

        private Vector3 CalculateDirection()
        {
            Vector3 moveDirectionForward = transform.forward * deltaMoveValue.y * Time.deltaTime;
            Vector3 moveDirectionSide = transform.right * deltaMoveValue.x * Time.deltaTime;
            Vector3 moveDirection = moveDirectionForward + moveDirectionSide;
            return moveDirection;
        }

        private void OnDisable()
        {
            InputEventHandler.JoystickMoveEvent -= JoystickMove;
            InputEventHandler.JoystickMoveInputEvent -= JoystickMoveInput;
        }
    }
}