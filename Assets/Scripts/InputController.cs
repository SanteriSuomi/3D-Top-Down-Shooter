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
            PlayerMove androidPlayerMove = FindObjectOfType<PlayerMove>();
            PlayerLook androidPlayerLook = FindObjectOfType<PlayerLook>();

            if (Input.mousePresent)
            {
                InitializePCEnvironment(pcControls, androidPlayerMove, androidPlayerLook);
            }
        }

        private void InitializePCEnvironment(PlayerControlsPC pcControls, PlayerMove androidPlayerMove, PlayerLook androidPlayerLook)
        {
            foreach (GameObject joystick in joysticks)
            {
                joystick.SetActive(false);
            }

            androidPlayerMove.enabled = false;
            androidPlayerLook.enabled = false;
            pcControls.enabled = true;

            crosshair.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }
}