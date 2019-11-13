using Shooter.Utility;
using UnityEngine;

namespace Shooter.AI
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        public float Hitpoints { get; set; }
        [SerializeField]
        protected float initialHitPoints = 100;

        protected abstract void InitializeState();
        private void Awake()
        {
            Hitpoints = initialHitPoints;
            InitializeState();
        }

        protected abstract void StartState();
        private void Start()
        {
            StartState();
        }

        protected abstract void UpdateState();
        private void Update()
        {
            CheckHitpoints();
            UpdateState();
        }

        public void TakeDamage(float damage)
        {
            Hitpoints -= damage;
        }

        public void CheckHitpoints()
        {
            if (Hitpoints <= float.Epsilon)
            {
                OnZeroHP();
            }
        }

        protected abstract void OnZeroHP();
    }
}