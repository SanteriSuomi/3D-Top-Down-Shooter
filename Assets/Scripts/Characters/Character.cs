using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.AI
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        public float Hitpoints { get; set; }
        [SerializeField]
        protected float hitPoints = 100;
        protected readonly int playerLayer = 9;

        protected abstract void InitializeState();
        private void Awake()
        {
            Hitpoints = hitPoints;
            InitializeState();
        }

        protected abstract void UpdateState();
        private void Update()
        {
            Die();
            UpdateState();
        }

        public void TakeDamage(float damage)
        {
            Hitpoints -= damage;
        }

        public void Die()
        {
            if (Hitpoints <= float.Epsilon)
            {
                if (gameObject.layer == playerLayer)
                {
                    Scene currentScene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}