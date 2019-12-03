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
            // Update score text every time playerSettings funds get updated.
            scoreText.text = $"{scoreTextString}{score}";
        }

        public override void OnDisable()
        {
            if (PlayerSettings.GetInstance() != null)
            {
                PlayerSettings.GetInstance().OnScoreChangeEvent -= OnScoreChange;
            }
        }
    }
}