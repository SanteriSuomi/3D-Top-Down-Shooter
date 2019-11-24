using System.Collections;
using TMPro;
using UnityEngine;

namespace Shooter.Shop
{
    public class FundsOut : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI fundsOutText;
        private WaitForSeconds buyFailedPopUp;
        private bool isFundsCoroutineRunning;

        private void Awake()
        {
            buyFailedPopUp = new WaitForSeconds(2);
            NoFundsEventHandler.OnFundsOutEvent += OnFundsOut;
        }

        private void OnFundsOut(string text)
        {
            if (!isFundsCoroutineRunning)
            {
                StartCoroutine(BuyFailedPopupCoroutine(text));
            }
        }

        private IEnumerator BuyFailedPopupCoroutine(string text)
        {
            fundsOutText.text = text;
            fundsOutText.gameObject.SetActive(true);
            yield return buyFailedPopUp;
            fundsOutText.gameObject.SetActive(false);
            isFundsCoroutineRunning = false;
        }
    }
}