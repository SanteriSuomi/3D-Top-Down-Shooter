using Shooter.Player;
using UnityEngine;

namespace Shooter.Inputs
{
    public class PlayerLook : MonoBehaviour
    {
        private PlayerSettings playerSettings;
        private Vector2 deltaLookValue;
        private bool joystickLook;
        private float rotation;
        [SerializeField]
        private float rotationSpeed = 10;
        [SerializeField]
        [Range(0, 1)]
        private float rotationSmooth = 0.8f;

        private void Awake()
        {
            playerSettings = GetComponent<PlayerSettings>();
            InputEventHandler.JoystickLookEvent += JoystickLook;
            InputEventHandler.JoystickLookInputEvent += JoystickLookInput;
        }

        private void JoystickLook(bool look)
        {
            joystickLook = look;
        }

        private void JoystickLookInput(Vector2 delta)
        {
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
            rotation += deltaLookValue.x * rotationSpeed * playerSettings.PlayerSensitivityMultiplier * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotation, 0), rotationSmooth);
        }

        private void OnDestroy()
        {
            InputEventHandler.JoystickLookEvent -= JoystickLook;
            InputEventHandler.JoystickLookInputEvent -= JoystickLookInput;
        }
    }
}