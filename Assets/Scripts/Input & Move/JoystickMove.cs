using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Input
{
    public class JoystickMove : JoystickBase
    {
        protected override void Awake()
        {
            base.Awake();
            inputActions.Player.TouchPositionMove.performed += TouchPositionMovePerformed;
        }

        protected virtual void TouchPositionMovePerformed(InputAction.CallbackContext inputAction)
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