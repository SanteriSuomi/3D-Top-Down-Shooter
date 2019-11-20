using Shooter.Utility;
using UnityEngine;

namespace Shooter.AI
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        public float HitPoints { get; set; }
        [SerializeField]
        protected float startingHitPoints = 100;

        protected abstract void InitializeState();
        private void Awake()
        {
            HitPoints = startingHitPoints;
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
            HitPoints -= damage;
        }

        public void CheckHitpoints()
        {
            if (HitPoints <= float.Epsilon)
            {
                OnZeroHP();
            }
        }

        protected abstract void OnZeroHP();
    }
}