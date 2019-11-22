using Shooter.Player;
using System.Collections;
using UnityEngine;

namespace Shooter.Shop
{
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField]
        private GameObject fundsOutText = default;
        [SerializeField]
        private float fundCost = 50;
        [SerializeField]
        private float amountToHeal = 100;
        private bool isFundsCoroutineRunning;

        public void HealthButton()
        {
            if (PlayerSettings.GetInstance() != null && PlayerSettings.GetInstance().Funds >= fundCost)
            {
                PlayerSettings.GetInstance().Funds -= fundCost;
                PlayerSettings.GetInstance().HitPoints = amountToHeal;
                Objective.GetInstance().HitPoints = amountToHeal * 5;
            }
            else
            {
                #if UNITY_EDITOR
                Debug.Log($"You don't have the required amount of funds.");
                #endif
                if (!isFundsCoroutineRunning)
                {
                    isFundsCoroutineRunning = true;
                    StartCoroutine(FundsOutText());
                }
            }
        }

        private IEnumerator FundsOutText()
        {
            fundsOutText.SetActive(true);
            yield return new WaitForSeconds(2);
            fundsOutText.SetActive(false);
            isFundsCoroutineRunning = false;
        }
    }
}