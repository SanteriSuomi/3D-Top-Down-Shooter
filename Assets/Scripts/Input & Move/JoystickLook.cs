using UnityEngine;

namespace Shooter.Inputs
{
    public class JoystickLook : JoystickBase
    {
        protected Vector2 rotateValue;

        protected override void Update()
        {
            base.Update();
            rotateValue = currentTouch.deltaPosition;
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