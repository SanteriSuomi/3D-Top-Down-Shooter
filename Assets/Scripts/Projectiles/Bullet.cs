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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IDamageable enemy))
            {
                enemy.Hitpoints -= bulletDamage;
                DeactivateAndEnqueue();
            }
        }

        private void OnBecameInvisible()
        {
            DeactivateAndEnqueue();
        }

        private void DeactivateAndEnqueue()
        {
            BulletPool.GetInstance().Enqueue(this);
        }
    }
}