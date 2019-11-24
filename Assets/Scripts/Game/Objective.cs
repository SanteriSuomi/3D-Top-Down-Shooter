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
                healthBar.value = hitpoints;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            HitPoints = startingHitpoints;
            playerCamera = Camera.main.transform;
        }

        private void Update()
        {
            CheckHitpoints();
            healthBar.transform.LookAt(playerCamera);
        }

        public void CheckHitpoints()
        {
            if (HitPoints <= float.Epsilon)
            {
                SceneManager.LoadScene(0);
            }
        }

        public void TakeDamage(float damage)
        {
            #if UNITY_EDITOR
            Debug.Log("Objective has taken damage");
            #endif
            HitPoints -= damage;
        }
    }
}