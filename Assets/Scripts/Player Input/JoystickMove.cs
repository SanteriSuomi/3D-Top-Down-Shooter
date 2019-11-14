namespace Shooter.Inputs
{
    public class JoystickMove : JoystickBase
    {
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