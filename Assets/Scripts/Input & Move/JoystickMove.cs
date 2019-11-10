using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Input
{
    public class JoystickMove : Joystick
    {
        protected override void Awake()
        {
            base.Awake();
            inputActions.Player.TouchPosition.performed += TouchPositionPerformed;
        }

        private void TouchPositionPerformed(InputAction.CallbackContext inputAction)
        {
            touchPosition = inputAction.ReadValue<Vector2>();
        }

        protected override void JoystickAction()
        {
            base.JoystickAction();
            InputEventHandler.InvokeJoystickMove(move: true);
            InputEventHandler.InvokeJoystickMoveInput(touchPositionToLocalRect);
        }

        protected override void CancelJoystickAction()
        {
            InputEventHandler.InvokeJoystickMove(move: false);
        }
    }
}