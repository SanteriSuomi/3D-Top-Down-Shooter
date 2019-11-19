using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerControlsPC : MonoBehaviour
    {
        private CharacterController m_CharacterController;
        private Camera m_Camera;
        private Vector3 m_CursorPos;
        [SerializeField]
        private float m_Speed = 5;
        [SerializeField]
        private RectTransform m_Crosshair;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
        }

        private void Update()
        {
            Move();
            HandleInputs();
            HandleRotation();
        }

        private void Move()
        {
            GetButtonInput(out float Horizontal, out float Vertical);
            Vector3 moveDirectionForward = transform.forward * Vertical;
            Vector3 moveDirectionSide = transform.right * Horizontal;
            Vector3 moveDirection = (moveDirectionForward + moveDirectionSide) * Time.deltaTime;
            m_CharacterController.Move(moveDirection * m_Speed);
        }

        private static void GetButtonInput(out float Horizontal, out float Vertical)
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
        }

        private void HandleInputs()
        {
            Ray screenRay = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(screenRay, out RaycastHit m_Hit))
            {
                m_CursorPos = m_Hit.point;
                m_CursorPos.y = 0;
                m_Crosshair.position = Input.mousePosition;
            }
        }

        private void HandleRotation()
        {
            transform.LookAt(m_CursorPos);
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        }
    }

}