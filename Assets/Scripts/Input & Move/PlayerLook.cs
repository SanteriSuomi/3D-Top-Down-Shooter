using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerLook : MonoBehaviour
    {
        private Vector2 deltaLookValue;
        private bool look;
        private bool joystickLook;
        private float rotation;
        [SerializeField]
        private float rotationSpeed = 10;
        [SerializeField]
        [Range(0, 1)]
        private float rotationSmooth = 0.8f;

        private void Awake()
        {
            InputEventHandler.JoystickLookEvent += JoystickLook;
            InputEventHandler.JoystickLookInputEvent += JoystickLookInput;
        }

        private void JoystickLook(bool look)
        {
            joystickLook = look;
        }

        private void JoystickLookInput(Vector2 delta)
        {
            deltaLookValue = delta;
            look = true;
            if (Mathf.Approximately(delta.x, float.Epsilon)
                || Mathf.Approximately(delta.y, float.Epsilon))
            {
                look = false;
            }
        }

        private void Update()
        {
            if (look && joystickLook)
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            rotation += deltaLookValue.x * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotation, 0), rotationSmooth);
        }

        private void OnDisable()
        {
            InputEventHandler.JoystickLookEvent -= JoystickLook;
            InputEventHandler.JoystickLookInputEvent -= JoystickLookInput;
        }
    }
}