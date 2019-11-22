using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerControlsPC : MonoBehaviour
    {
        private CharacterController m_CharacterController;
        private Camera m_Camera;
        private Vector3 m_CursorPos;
        [SerializeField]
        private float m_MovementSpeed = 5;
        [SerializeField]
        private RectTransform m_Crosshair = default;
        [SerializeField]
        private float rotationSpeed = 5;
        [SerializeField]
        private float minAngleFloatToRotate = 7.5f;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
        }

        private void Update()
        {
            Move();
            Rotation();
        }

        private void Move()
        {
            HandleMovementInput(out float horizontal, out float vertical);
            Vector3 moveDirectionSide = transform.right * horizontal;
            Vector3 moveDirectionForward = transform.forward * vertical;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide) * Time.deltaTime;
            moveDirection = ApplyGravity(moveDirection);
            m_CharacterController.Move(moveDirection * m_MovementSpeed);
        }

        private void HandleMovementInput(out float horizontal, out float vertical)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        private Vector3 ApplyGravity(Vector3 moveDirection)
        {
            if (!m_CharacterController.isGrounded)
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
            Vector3 relativePosition = (m_CursorPos - transform.position).normalized;
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
            Ray screenRay = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(screenRay, out RaycastHit m_Hit))
            {
                m_CursorPos = m_Hit.point;
                m_CursorPos.y = 0;
                m_Crosshair.position = Input.mousePosition;
            }
        }
    }
}