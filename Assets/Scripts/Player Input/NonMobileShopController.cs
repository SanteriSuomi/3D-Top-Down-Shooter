using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Shooter.Inputs
{
    public class NonMobileShopController : MonoBehaviour
    {
        //
        // Script that controls input for PC.
        //
        [SerializeField]
        private GameObject[] shopBuyButtons = default;
        private Button[] shopBuyButtonComponents;
        private InputActions inputActions;
        private bool isShopping = false;

        private void Awake()
        {
            inputActions = new InputActions();
            inputActions.Player.ShopEnable.performed += ShopEnablePerformed;
            inputActions.Player.ShopFollower.performed += ShopFollowerPerformed;
            inputActions.Player.ShopHealth.performed += ShopHealthPerformed;
        }

        private void Start()
        {
            if (!Input.mousePresent)
            {
                // When mouse is not present, this script should be disabled.
                enabled = false;
            }
            else if (Input.mousePresent)
            {
                // Else if the mouse is present, enable all the input controls in the input system.
                inputActions.Player.ShopEnable.Enable();
                inputActions.Player.ShopHealth.Enable();
                inputActions.Player.ShopFollower.Enable();
                InitializeButtonComponents();
            }
            else
            {
                Debug.LogError("Mouse or touch is not present.");
            }
        }

        private void InitializeButtonComponents()
        {
            // Initialize the array for buttons.
            shopBuyButtonComponents = new Button[shopBuyButtons.Length];
            // Get the button component scripts from the button gameObjects.
            for (int i = 0; i < shopBuyButtons.Length; i++)
            {
                shopBuyButtonComponents[i] = shopBuyButtons[i].GetComponent<Button>();
            }
        }

        private void ShopEnablePerformed(InputAction.CallbackContext context)
        {
            // Enable/Disable is shopping menu.
            isShopping = !isShopping;
            foreach (GameObject button in shopBuyButtons)
            {
                button.SetActive(!button.activeSelf);
            }
        }

        private void ShopFollowerPerformed(InputAction.CallbackContext obj)
        {
            // If shopping menus is activated.
            if (isShopping)
            {
                // Invoke the corresponding input button.
                shopBuyButtonComponents[0].onClick.Invoke();
            }
        }

        private void ShopHealthPerformed(InputAction.CallbackContext obj)
        {
            // If shopping menus is activated.
            if (isShopping)
            {
                // Invoke the corresponding input button.
                shopBuyButtonComponents[1].onClick.Invoke();
            }
        }

        private void OnDisable()
        {
            inputActions.Player.ShopEnable.Disable();
            inputActions.Player.ShopFollower.Disable();
            inputActions.Player.ShopHealth.Disable();
        }
    }
}