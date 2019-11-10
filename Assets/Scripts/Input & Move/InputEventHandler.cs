using UnityEngine;

namespace Shooter.Input
{
    public class InputEventHandler : MonoBehaviour
    {
        public delegate void JoystickMoveDelegate();
        public static event JoystickMoveDelegate JoystickMoveEvent;
        public static event JoystickMoveDelegate JoystickStopEvent;

        public delegate void JoystickInputDelegate(float delta);
        public static event JoystickInputDelegate JoystickInputEvent;

        public static void InvokeJoystickMove()
        {
            Debug.Log("InvokeJoystickMove");
            JoystickMoveEvent.Invoke();
        }

        public static void InvokeJoystickStop()
        {
            Debug.Log("InvokeJoystickStop");
            JoystickStopEvent.Invoke();
        }

        public static void InvokeJoystickInput(float delta)
        {
            JoystickInputEvent.Invoke(delta);
        }
    }
}