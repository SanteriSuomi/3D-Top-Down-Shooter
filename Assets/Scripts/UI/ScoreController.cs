using Shooter.Player;
using Shooter.Utility;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class ScoreController : GenericSingleton<ScoreController>
    {
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private string scoreTextString = "Score: ";

        protected override void Awake()
        {
            base.Awake();
            scoreText = GetComponent<TextMeshProUGUI>();
            PlayerSettings.GetInstance().OnScoreChangeEvent += OnScoreChange;
        }

        private void OnScoreChange(float score)
        {
            scoreText.text = $"{scoreTextString}{score}";
        }
    }
}