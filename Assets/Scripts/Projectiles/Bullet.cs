using UnityEngine;

namespace Shooter.Utility
{
    public class Bullet : MonoBehaviour
    {
        public Rigidbody BulletRigidBody { get; set; }
        [SerializeField]
        private float bulletDamage = 2.5f;

        private void Awake()
        {
            // Cache the rigidbody in a public property once, since it's going to be used a lot.
            BulletRigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            // Make sure collision is an enemy.
            if (collision.TryGetComponent(out IDamageable enemy))
            {
                enemy.HitPoints -= bulletDamage;
                DeactivateAndEnqueue();
            }
        }

        private void OnBecameInvisible()
        {
            // Re-pool the object when it becomes invisible.
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