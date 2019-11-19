using UnityEngine;

namespace Shooter.Inputs
{
    public class InputController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] joysticks = default;
        [SerializeField]
        private GameObject crosshair = default;
        private void Awake()
        {
            PlayerControlsPC pcControls = FindObjectOfType<PlayerControlsPC>();

            if (Input.mousePresent)
            {
                InitializePCEnvironment(pcControls);
            }
        }

        private void InitializePCEnvironment(PlayerControlsPC pcControls)
        {
            foreach (GameObject joystick in joysticks)
            {
                joystick.SetActive(false);
            }

            crosshair.SetActive(true);
            pcControls.enabled = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }
}