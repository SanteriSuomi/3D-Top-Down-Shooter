using UnityEngine;

namespace Shooter.Input
{
    public class InputEventHandler : MonoBehaviour
    {
        public delegate void JoystickMoveDelegate(bool move);
        public static event JoystickMoveDelegate JoystickMoveEvent;

        public delegate void JoystickInputDelegate(Vector2 delta);
        public static event JoystickInputDelegate JoystickInputEvent;

        public static void InvokeJoystickMove(bool move)
        {
            JoystickMoveEvent.Invoke(move);
        }

        public static void InvokeJoystickInput(Vector2 delta)
        {
            JoystickInputEvent.Invoke(delta);
        }
    }
}