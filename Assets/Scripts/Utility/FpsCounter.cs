using TMPro;
using UnityEngine;

namespace Shooter.Utility
{
    public class FpsCounter : MonoBehaviour
    {
        private TextMeshProUGUI fpsCounter;
        private float timer;
        [SerializeField]
        private float timerInterval = 0.25f;

        private void Awake()
        {
            fpsCounter = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timerInterval)
            {
                timer = 0;
                DisplayFPS();
            }
        }

        private void DisplayFPS()
        {
            float fps = Mathf.RoundToInt(1 / Time.deltaTime);
            fpsCounter.SetText($"{fps}");
        }
    }
}