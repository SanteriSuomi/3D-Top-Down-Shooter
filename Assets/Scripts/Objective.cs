using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shooter.Player
{
    public class Objective : GenericSingleton<Objective>, IDamageable
    {
        [SerializeField]
        private Slider healthBar = default;

        [SerializeField]
        private float startingHitpoints = 100;
        private float hitpoints;
        public float HitPoints 
        {
            get { return hitpoints; }
            set { hitpoints = value; healthBar.value = hitpoints; } 
        }

        private void Awake()
        {
            base.Awake();
            HitPoints = startingHitpoints;
        }

        private void Update()
        {
            CheckHitpoints();
        }

        public void CheckHitpoints()
        {
            if (HitPoints <= 0)
            {
                SceneManager.LoadScene(0);
                #if UNITY_EDITOR
                Debug.Log("Objective has taken damage");
                #endif
            }
        }

        public void TakeDamage(float damage)
        {
            HitPoints -= damage;
        }
    }
}