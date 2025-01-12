﻿using UnityEngine;

namespace Shooter.Inputs
{
    public class InputEventHandler : MonoBehaviour
    {
        //
        // InputEventHandler is a static class for handling the eventing between the player move/look and the joysticks.
        //

        #region Joystick Move Delegate/Event Intermediaries
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
        #endregion

        #region Joystick Look Delegate/Event Intermediaries
        public delegate void JoystickLookDelegate(bool look);
        public static event JoystickLookDelegate JoystickLookEvent;

        public delegate void JoystickLookInputDelegate(Vector2 delta);
        public static event JoystickLookInputDelegate JoystickLookInputEvent;

        public static void InvokeJoystickLook(bool look)
        {
            JoystickLookEvent.Invoke(look);
        }

        public static void InvokeJoystickLookInput(Vector2 delta)
        {
            JoystickLookInputEvent.Invoke(delta);
        }
        #endregion
    }
}