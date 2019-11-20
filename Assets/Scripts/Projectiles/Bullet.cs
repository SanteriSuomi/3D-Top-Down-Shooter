using UnityEngine;

namespace Shooter.Utility
{
    public class Bullet : MonoBehaviour
    {
        public Rigidbody RigidBody { get; set; }
        [SerializeField]
        private float bulletDamage = 2.5f;

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.TryGetComponent(out IDamageable enemy))
            {
                enemy.HitPoints -= bulletDamage;
                DeactivateAndEnqueue();
            }
        }

        private void OnBecameInvisible()
        {
            DeactivateAndEnqueue();
        }

        private void DeactivateAndEnqueue()
        {
            if (BulletPool.GetInstance() != null)
            {
                BulletPool.GetInstance().Enqueue(this);
            }
        }
    }
}