using Shooter.Player;
using Shooter.Utility;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class FundsController : GenericSingleton<FundsController>
    {
        [SerializeField]
        private string fundsTextString = "Prayers";
        private TextMeshProUGUI fundsText;

        protected override void Awake()
        {
            base.Awake();
            fundsText = GetComponent<TextMeshProUGUI>();
            PlayerSettings.GetInstance().OnFundsChangeEvent += OnFundsChange;
        }

        private void OnFundsChange(float funds)
        {
            fundsText.text = $"{fundsTextString}: {funds}";
        }
    }
}