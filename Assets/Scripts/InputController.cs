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
                foreach (GameObject joystick in joysticks)
                {
                    joystick.SetActive(false);
                }

                crosshair.SetActive(true);

                pcControls.enabled = true;
            }
        }
    }
}