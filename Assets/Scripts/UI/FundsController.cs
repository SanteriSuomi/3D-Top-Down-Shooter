using Shooter.Player;
using Shooter.Utility;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class FundsController : GenericSingleton<FundsController>
    {
        [SerializeField]
        private string fundsTextString = "Funds";
        private TextMeshProUGUI fundsText;

        protected override void Awake()
        {
            base.Awake();
            fundsText = GetComponent<TextMeshProUGUI>();
            if (PlayerSettings.GetInstance() != null)
            {
                PlayerSettings.GetInstance().OnFundsChangeEvent += OnFundsChange;
            }
        }

        private void OnFundsChange(float funds)
        {
            fundsText.text = $"{fundsTextString}: {funds}";
        }

        private void OnDestroy()
        {
            if (PlayerSettings.GetInstance() != null)
            {
                PlayerSettings.GetInstance().OnFundsChangeEvent -= OnFundsChange;
            }
        }
    }
}