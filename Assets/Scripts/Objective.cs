using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.Player
{
    public class Objective : MonoBehaviour, IDamageable
    {
        public float Hitpoints { get; set; }
        [SerializeField]
        private float startingHitpoints = 100;

        private void Awake()
        {
            Hitpoints = startingHitpoints;
        }

        private void Update()
        {
            CheckHitpoints();
        }

        public void CheckHitpoints()
        {
            if (Hitpoints <= 0)
            {
                SceneManager.LoadScene(0);
                #if UNITY_EDITOR
                Debug.Log("Objective has taken damage");
                #endif
            }
        }

        public void TakeDamage(float damage)
        {
            Hitpoints -= damage;
        }
    }
}