using UnityEngine;

namespace Shooter.Input
{
    public class PlayerLook : MonoBehaviour
    {
        private Vector2 deltaLookValue;
        private bool look;
        private bool joystickLook;

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
            Quaternion direction = Quaternion.LookRotation(deltaLookValue);
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, 0.5f);
        }

        private void OnDisable()
        {
            InputEventHandler.JoystickLookEvent -= JoystickLook;
            InputEventHandler.JoystickLookInputEvent -= JoystickLookInput;
        }
    }
}