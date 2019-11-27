using UnityEngine;

namespace Shooter.Inputs
{
    public class JoystickMove : JoystickBase
    {
        protected override void JoystickAction()
        {
            // JoystickAction is called when touch input is registered on this joystick.
            base.JoystickAction();
            InputEventHandler.InvokeJoystickMove(move: true);
            InputEventHandler.InvokeJoystickMoveInput(touchPositionToLocalRect);
        }

        protected override void CancelJoystickAction()
        {
            // CancelJoystickAction is called while input is not being registered on this joystick.
            InputEventHandler.InvokeJoystickMove(move: false);
            InputEventHandler.InvokeJoystickMoveInput(Vector2.zero);
        }
    }
}