using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Input
{
    public class JoystickLook : Joystick
    {
        private Vector2 rotateValue;

        protected override void Awake()
        {
            base.Awake();
            inputActions.Player.Look.performed += LookPerformed;
        }

        private void LookPerformed(InputAction.CallbackContext inputAction)
        {
            rotateValue = inputAction.ReadValue<Vector2>();
        }

        protected override void JoystickAction()
        {
            base.JoystickAction();
            InputEventHandler.InvokeJoystickLook(move: true);
            InputEventHandler.InvokeJoystickLookInput(rotateValue);
        }

        protected override void CancelJoystickAction()
        {
            InputEventHandler.InvokeJoystickLook(move: false);
        }
    }
}