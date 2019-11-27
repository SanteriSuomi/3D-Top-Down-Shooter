using Shooter.Player;
using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerLook : MonoBehaviour
    {
        private PlayerSettings playerSettings;
        private Vector2 deltaLookValue;
        [SerializeField]
        private float rotationSpeed = 10;
        [SerializeField]
        [Range(0, 1)]
        private float rotationSmooth = 0.8f;
        private float rotation;
        private bool joystickLook;

        private void Awake()
        {
            playerSettings = GetComponent<PlayerSettings>();
            InputEventHandler.JoystickLookEvent += JoystickLook;
            InputEventHandler.JoystickLookInputEvent += JoystickLookInput;
        }

        private void JoystickLook(bool look)
        {
            // Method that controls whether or not looking should be activated.
            joystickLook = look;
        }

        private void JoystickLookInput(Vector2 delta)
        {
            // The value from the joystick handling the rotation.
            deltaLookValue = delta;
        }

        private void Update()
        {
            if (joystickLook)
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            // Update rotation by adding the required values to it.
            rotation += deltaLookValue.x * rotationSpeed * playerSettings.PlayerSensitivityMultiplier * Time.deltaTime;
            // Update the rotation with the rotation value.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotation, 0), rotationSmooth);
        }

        private void OnDisable()
        {
            InputEventHandler.JoystickLookEvent -= JoystickLook;
            InputEventHandler.JoystickLookInputEvent -= JoystickLookInput;
        }
    }
}