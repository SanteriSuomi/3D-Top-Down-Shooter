using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerControlsPC : MonoBehaviour
    {
        private CharacterController characterController;
        private Camera mainCamera;
        private Vector3 cursorHitPosition;
        [SerializeField]
        private RectTransform crossHair = default;
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
        }

        private void Update()
        {
            Crosshair();
            Move();
            Rotation();
        }

        private void Crosshair()
        {
            crossHair.position = Input.mousePosition;
        }

        private void Move()
        {
            HandleMovementInput(out float horizontal, out float vertical);
            Vector3 moveDirectionSide = transform.right * horizontal;
            Vector3 moveDirectionForward = transform.forward * vertical;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide) * Time.deltaTime;
            moveDirection = ApplyGravity(moveDirection);
            characterController.Move(moveDirection * movementSpeed);
        }

        private void HandleMovementInput(out float horizontal, out float vertical)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
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
            HandleRotationInput();
            Vector3 relativePosition = (cursorHitPosition - transform.position).normalized;
            Quaternion lookDirection = Quaternion.LookRotation(relativePosition, Vector3.up);
            float angleBetweenRotations = Quaternion.Angle(transform.rotation, lookDirection);
            if (angleBetweenRotations >= minAngleFloatToRotate)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.Euler(new Vector3(0, lookDirection.eulerAngles.y, 0)),
                    rotationSpeed * Time.deltaTime);
            }
        }

        private void HandleRotationInput()
        {
            Ray screenRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(screenRay, out RaycastHit hit))
            {
                cursorHitPosition = hit.point;
                cursorHitPosition.y = 0;
            }
        }
    }
}