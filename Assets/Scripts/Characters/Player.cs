using Shooter.AI;
using UnityEngine.SceneManagement;

namespace Shooter.Player
{
    public class Player : Character
    {
        private PlayerSettings playerSettings;

        protected override void InitializeState()
        {
            playerSettings = GetComponent<PlayerSettings>();
            playerSettings.OnHitpointChangeEvent += OnHitpointsChange;
        }

        private void OnHitpointsChange(float hitPoints)
        {
            Hitpoints = hitPoints;
        }

        protected override void StartState()
        {
            Hitpoints = startingHitPoints;
        }

        protected override void UpdateState()
        {

        }

        protected override void OnZeroHP()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDisable()
        {
            playerSettings.OnHitpointChangeEvent -= OnHitpointsChange;
        }
    }
}