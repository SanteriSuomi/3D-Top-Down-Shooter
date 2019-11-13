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

        private void Update()
        {
            fundsText.text = $"{fundsTextString}: {Funds}";
        }
    }
}