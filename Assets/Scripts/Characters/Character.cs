using Photon.Pun;
using Shooter.Utility;
using UnityEngine;

namespace Shooter.AI
{
    public abstract class Character : MonoBehaviourPunCallbacks, IDamageable
    {
        public float HitPoints { get; set; }
        [SerializeField]
        protected float startingHitPoints = 100;

        protected abstract void InitializeState();
        private void Awake()
        {
            // Initialize the global hitpoints with a starting hitpoints.
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
            // Constantly check if hitpoints are below a certain threshold.
            CheckHitpoints();
            UpdateState();
        }

        protected virtual void OnTakeDamage(float damage)
        {
            // Method to be called with TakeDamage, but can also be used independently.
            HitPoints -= damage;
        }
        public void TakeDamage(float damage)
        {
            OnTakeDamage(damage);
        }

        protected abstract void OnZeroHP();
        public void CheckHitpoints()
        {
            // If hitpoints are equal or below zero, activate the abstract OnZeroHP method.
            if (HitPoints <= float.Epsilon)
            {
                OnZeroHP();
            }
        }
    }
}