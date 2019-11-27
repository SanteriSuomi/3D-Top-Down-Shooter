using UnityEngine;

namespace Shooter.Inputs
{
    public class InputTypeManager : MonoBehaviour
    {
        //
        // InputTypeManager handles enabling/disabling scripts for different input systems.
        //

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
                // If there is no mouse, disable this script.
                enabled = false;
            }
            else if (Input.mousePresent)
            {
                // Else if there is a mouse, initialize the PC environment.
                FindScriptComponents(out NonMobilePlayerControls pcControls, out PlayerMove androidPlayerMove, out PlayerLook androidPlayerLook);
                InitializePCEnvironment(pcControls, androidPlayerMove, androidPlayerLook);
            }
            else
            {
                Debug.LogError("No touch/mouse input found.");
            }
        }

        private static void FindScriptComponents(out NonMobilePlayerControls pcControls, out PlayerMove androidPlayerMove, out PlayerLook androidPlayerLook)
        {
            pcControls = FindObjectOfType<NonMobilePlayerControls>();
            androidPlayerMove = FindObjectOfType<PlayerMove>();
            androidPlayerLook = FindObjectOfType<PlayerLook>();
        }

        private void InitializePCEnvironment(NonMobilePlayerControls pcControls, PlayerMove androidPlayerMove, PlayerLook androidPlayerLook)
        {
            // Deactivate all the joysticks.
            foreach (GameObject joystick in joysticks)
            {
                joystick.SetActive(false);
            }
            // Activate all the PC info bars.
            foreach (GameObject info in pcButtonInfos)
            {
                info.SetActive(true);
            }
            // Disable the scripts associated with android.
            androidPlayerMove.enabled = false;
            androidPlayerLook.enabled = false;
            // And enable the scripts associated with PC.
            pcControls.enabled = true;
            // Activate the crosshair.
            crosshair.SetActive(true);
            // Lock the cursor and make it invisible.
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }
}