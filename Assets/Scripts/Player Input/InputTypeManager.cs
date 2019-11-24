using UnityEngine;

namespace Shooter.Inputs
{
    public class InputTypeManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] joysticks = default;
        [SerializeField]
        private GameObject[] pcButtonInfos = default;
        [SerializeField]
        private GameObject crosshair = default;

        private void Start()
        {
            if (!Input.mousePresent)
            {
                enabled = false;
            }
            else
            {
                NonMobilePlayerControls pcControls = FindObjectOfType<NonMobilePlayerControls>();
                PlayerMove androidPlayerMove = FindObjectOfType<PlayerMove>();
                PlayerLook androidPlayerLook = FindObjectOfType<PlayerLook>();
                InitializePCEnvironment(pcControls, androidPlayerMove, androidPlayerLook);
            }
        }

        private void InitializePCEnvironment(NonMobilePlayerControls pcControls, PlayerMove androidPlayerMove, PlayerLook androidPlayerLook)
        {
            foreach (GameObject joystick in joysticks)
            {
                joystick.SetActive(false);
            }

            foreach (GameObject info in pcButtonInfos)
            {
                info.SetActive(true);
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