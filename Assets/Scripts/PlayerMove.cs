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
        private float moveSpeed = 0.1f;
        private bool move;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            inputActions = new InputActions();
            inputActions.Player.Move.performed += MovePerformed;
            inputActions.Player.Move.canceled += MoveCanceled;
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

        private void MoveCanceled(InputAction.CallbackContext obj)
        {
            move = false;
        }

        private void Update()
        {
            if (move)
            {
                Move();
            }
        }

        private void Move()
        {
            Vector3 moveDirectionForward = transform.forward * inputValue.y * Time.deltaTime;
            Vector3 moveDirectionSide = transform.right * inputValue.x * Time.deltaTime;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide).normalized;
            characterController.Move(moveDirection * moveSpeed);
            Debug.Log($"MovePerformed, {moveDirection * moveSpeed}");
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }
    }
}