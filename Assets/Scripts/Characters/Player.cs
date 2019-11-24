using Shooter.AI;
using UnityEngine;
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
            HitPoints = hitPoints;
        }

        protected override void StartState()
        {
            HitPoints = startingHitPoints;
        }

        protected override void UpdateState()
        {
            
        }

        protected override void OnTakeDamage(float damage)
        {
            base.OnTakeDamage(damage);
            playerSettings.HitPoints -= damage;
        }

        protected override void OnZeroHP()
        {
            #if UNITY_STANDALONE
            Cursor.visible = true;
            #endif
            SceneManager.LoadScene(0);
        }

        private void OnDisable()
        {
            playerSettings.OnHitpointChangeEvent -= OnHitpointsChange;
        }
    }
}