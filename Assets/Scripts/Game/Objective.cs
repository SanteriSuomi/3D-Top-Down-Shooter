using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shooter.Player
{
    public class Objective : GenericSingleton<Objective>, IDamageable
    {
        private Transform playerCamera;
        [SerializeField]
        private Slider healthBar = default;
        [SerializeField]
        private float startingHitpoints = 100;
        private float hitpoints;
        public float HitPoints
        {
            get { return hitpoints; }
            set
            {
                hitpoints = value; 
                // Update the healthbar value too.
                healthBar.value = hitpoints;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            // Hitpoints should start with these hitpoints.
            HitPoints = startingHitpoints;
            playerCamera = Camera.main.transform;
        }

        private void Update()
        {
            CheckHitpoints();
            UpdateRotation();
        }

        public void CheckHitpoints()
        {
            // If objective gets destroyed, go back to main menu.
            if (HitPoints <= float.Epsilon)
            {
                SceneManager.LoadScene(0);
            }
        }

        private void UpdateRotation()
        {
            // Rotate healthbar towards player camera.
            healthBar.transform.LookAt(playerCamera);
        }

        public void TakeDamage(float damage)
        {
            HitPoints -= damage;
        }
    }
}