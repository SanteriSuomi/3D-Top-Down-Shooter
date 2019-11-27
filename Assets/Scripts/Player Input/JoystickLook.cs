using UnityEngine;

namespace Shooter.Inputs
{
    public class JoystickLook : JoystickBase
    {
        private Vector2 rotateValue;

        protected override void Update()
        {
            base.Update();
            // rotateValue controls the rotating of the character.
            rotateValue = currentTouch.deltaPosition;
        }

        protected override void JoystickAction()
        {
            // JoystickAction is called when touch input is registered on this joystick.
            base.JoystickAction();
            InputEventHandler.InvokeJoystickLook(look: true);
            InputEventHandler.InvokeJoystickLookInput(rotateValue);
        }

        protected override void CancelJoystickAction()
        {
            // CancelJoystickAction is called while input is not being registered on this joystick.
            InputEventHandler.InvokeJoystickLook(look: false);
            InputEventHandler.InvokeJoystickLookInput(Vector2.zero);
        }
    }
}