using UnityEngine;

namespace Shooter.Inputs
{
    public class InputController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] joysticks = default;
        private void Awake()
        {
            if (Input.mousePresent)
            {
                foreach (GameObject joystick in joysticks)
                {
                    joystick.SetActive(false);
                }
            }
        }
    }
}