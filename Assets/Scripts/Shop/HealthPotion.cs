using Shooter.Player;
using UnityEngine;

namespace Shooter.Shop
{
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField]
        private ShopText shopText = default;
        [SerializeField]
        private float fundCost = 50;
        [SerializeField]
        private float amountToHeal = 100;
        private float originalObjectiveHitPoints;

        private void Awake()
        {
            originalObjectiveHitPoints = Objective.GetInstance().HitPoints;
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
                NoFundsEventHandler.TriggerFundsOutPopUp(shopText.FundsOut);
            }
        }
    }
}