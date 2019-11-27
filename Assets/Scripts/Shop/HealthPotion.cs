using Shooter.Player;
using System.Collections;
using UnityEngine;

namespace Shooter.Shop
{
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField]
        private GameObject fundsOutText = default;
        private WaitForSeconds fundsOutTextTimer;
        [SerializeField]
        private float fundsOutTextTime = 2;
        private float originalObjectiveHitPoints;
        [SerializeField]
        private float fundCost = 50;
        [SerializeField]
        private float amountToHeal = 100;
        private bool isFundsCoroutineRunning;

        private void Awake()
        {
            originalObjectiveHitPoints = Objective.GetInstance().HitPoints;
            fundsOutTextTimer = new WaitForSeconds(fundsOutTextTime);
        }

        public void HealthButton()
        {
            // Make sure the playersettings exists and there are enough funds.
            if (PlayerSettings.GetInstance() != null && PlayerSettings.GetInstance().Funds >= fundCost)
            {
                // Apply healthpotion to the objective and player.
                PlayerSettings.GetInstance().Funds -= fundCost;
                PlayerSettings.GetInstance().HitPoints = amountToHeal;
                Objective.GetInstance().HitPoints = originalObjectiveHitPoints;
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
            // Show the funds out text.
            fundsOutText.SetActive(true);
            yield return fundsOutTextTimer;
            fundsOutText.SetActive(false);
            isFundsCoroutineRunning = false;
        }
    }
}