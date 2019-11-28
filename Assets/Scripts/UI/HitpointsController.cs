using UnityEngine;
using TMPro;

namespace Shooter.Player
{
    public class HitpointsController : MonoBehaviour
    {
        private TextMeshProUGUI hitpointsText;
        [SerializeField]
        private string hitpointsTextString = "Hitpoints: ";

        private void Awake()
        {
            hitpointsText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            PlayerSettings.GetInstance().OnHitpointChangeEvent += OnHitpointChange;
        }

        private void OnHitpointChange(float hitPoints)
        {
            // Update hitpoints text every time playerSettings funds get updated.
            hitpointsText.text = $"{hitpointsTextString}{hitPoints}";
        }

        private void OnDisable()
        {
            if (PlayerSettings.GetInstance() != null)
            {
                PlayerSettings.GetInstance().OnHitpointChangeEvent -= OnHitpointChange;
            }
        }
    }
}