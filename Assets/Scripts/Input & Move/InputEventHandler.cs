using UnityEngine;

namespace Shooter.Input
{
    public class InputEventHandler : MonoBehaviour
    {
        public delegate void JoystickDelegate();
        public static event JoystickDelegate JoystickMoveEvent;
        public static event JoystickDelegate JoystickStopEvent;

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
    }
}