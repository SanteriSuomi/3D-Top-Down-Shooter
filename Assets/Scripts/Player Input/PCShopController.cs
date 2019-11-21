using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Inputs
{
    public class PCShopController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] shopBuyButtons = default;
        [SerializeField]
        private Button[] shopBuyButtonComponents = default;
        [SerializeField]
        private KeyCode shopButtonKey = KeyCode.E;
        [SerializeField]
        private KeyCode shopButtonFollowerKey = KeyCode.F;
        [SerializeField]
        private KeyCode shopButtonHealthKey = KeyCode.H;
        private bool isShopping = false;

        private void Start()
        {
            if (!Input.mousePresent)
            {
                enabled = false;
            }

            InitializeButtonComponents();
        }

        private void InitializeButtonComponents()
        {
            for (int i = 0; i < shopBuyButtons.Length; i++)
            {
                shopBuyButtonComponents[i] = shopBuyButtons[i].GetComponent<Button>();
            }
        }

        private void Update()
        {
            ShopInput();
        }

        private void ShopInput()
        {
            if (Input.GetKeyDown(shopButtonKey))
            {
                isShopping = !isShopping;
                foreach (GameObject button in shopBuyButtons)
                {
                    button.SetActive(!button.activeSelf);
                }
            }
            else if (isShopping && Input.GetKeyDown(shopButtonFollowerKey))
            {
                shopBuyButtonComponents[0].onClick.Invoke();
            }
            else if (isShopping && Input.GetKeyDown(shopButtonHealthKey))
            {
                shopBuyButtonComponents[1].onClick.Invoke();
            }
        }
    }
}