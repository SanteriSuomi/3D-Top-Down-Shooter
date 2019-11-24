using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Shooter.Inputs
{
    public class NonMobileShopController : MonoBehaviour
    {
        private InputActions inputActions;
        [SerializeField]
        private GameObject[] shopBuyButtons = default;
        private Button[] shopBuyButtonComponents;
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
                enabled = false;
            }
            else
            {
                inputActions.Player.ShopEnable.Enable();
                inputActions.Player.ShopHealth.Enable();
                inputActions.Player.ShopFollower.Enable();
            }

            InitializeButtonComponents();
        }

        private void InitializeButtonComponents()
        {
            shopBuyButtonComponents = new Button[shopBuyButtons.Length];
            for (int i = 0; i < shopBuyButtons.Length; i++)
            {
                shopBuyButtonComponents[i] = shopBuyButtons[i].GetComponent<Button>();
            }
        }

        private void ShopFollowerPerformed(InputAction.CallbackContext obj)
        {
            if (isShopping)
            {
                shopBuyButtonComponents[0].onClick.Invoke();
            }
        }

        private void ShopHealthPerformed(InputAction.CallbackContext obj)
        {
            if (isShopping)
            {
                shopBuyButtonComponents[1].onClick.Invoke();
            }
        }

        private void ShopEnablePerformed(InputAction.CallbackContext context)
        {
            isShopping = !isShopping;
            foreach (GameObject button in shopBuyButtons)
            {
                button.SetActive(!button.activeSelf);
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