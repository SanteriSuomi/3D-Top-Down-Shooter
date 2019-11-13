using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.AI
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        public float Hitpoints { get; set; }
        [SerializeField]
        protected float awakeHitPoints = 100;
        protected readonly string playerTag = "Player";

        protected abstract void InitializeState();
        private void Awake()
        {
            Hitpoints = awakeHitPoints;
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
                if (gameObject.CompareTag(playerTag))
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