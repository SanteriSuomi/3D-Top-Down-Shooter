using Shooter.AI;
using UnityEngine.SceneManagement;

namespace Shooter.Player
{
    public class Player : Character
    {
        protected override void InitializeState()
        {
            GetComponent<PlayerSettings>().OnHitpointChangeEvent += OnHitpointsChange;
        }

        private void OnHitpointsChange(float hitPoints)
        {
            Hitpoints = hitPoints;
        }

        protected override void StartState()
        {
            Hitpoints = initialHitPoints;
        }

        protected override void UpdateState()
        {

        }

        protected override void OnZeroHP()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}