using UnityEngine;

namespace Shooter.Input
{
    public class InputEventHandler : MonoBehaviour
    {
        public delegate void JoystickMoveDelegate(bool move);
        public static event JoystickMoveDelegate JoystickMoveEvent;

        public delegate void JoystickMoveInputDelegate(Vector2 delta);
        public static event JoystickMoveInputDelegate JoystickMoveInputEvent;

        public static void InvokeJoystickMove(bool move)
        {
            JoystickMoveEvent.Invoke(move);
        }

        public static void InvokeJoystickMoveInput(Vector2 delta)
        {
            JoystickMoveInputEvent.Invoke(delta);
        }

        public delegate void JoystickLookDelegate(bool move);
        public static event JoystickLookDelegate JoystickLookEvent;

        public delegate void JoystickLookInputDelegate(Vector2 delta);
        public static event JoystickLookInputDelegate JoystickLookInputEvent;

        public static void InvokeJoystickLook(bool move)
        {
            JoystickLookEvent.Invoke(move);
        }

        public static void InvokeJoystickLookInput(Vector2 delta)
        {
            JoystickLookInputEvent.Invoke(delta);
        }
    }
}