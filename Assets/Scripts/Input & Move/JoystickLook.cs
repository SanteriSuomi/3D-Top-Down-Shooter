using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Inputs
{
    public class JoystickLook : JoystickBase
    {
        private Vector2 rotateValue;

        protected override void Awake()
        {
            base.Awake();
            inputActions.Player.TouchPositionLook.performed += TouchPositionLookPerformed;
            inputActions.Player.Look.performed += LookPerformed;
        }

        protected virtual void TouchPositionLookPerformed(InputAction.CallbackContext inputAction)
        {
            //touchPosition = inputAction.ReadValue<Vector2>();
        }

        private void LookPerformed(InputAction.CallbackContext inputAction)
        {
            rotateValue = inputAction.ReadValue<Vector2>();
        }

        protected override void JoystickAction()
        {
            base.JoystickAction();
            InputEventHandler.InvokeJoystickLook(look: true);
            InputEventHandler.InvokeJoystickLookInput(rotateValue);
        }

        protected override void CancelJoystickAction()
        {
            InputEventHandler.InvokeJoystickLook(look: false);
        }
    }
}