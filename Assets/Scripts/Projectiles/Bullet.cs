using System.Collections;
using UnityEngine;

namespace Shooter.Utility
{
    public class Bullet : MonoBehaviour
    {
        public Rigidbody RigidBody { get; set; }
        [SerializeField]
        private float bulletDamage = 2.5f;
        [SerializeField]
        private float deactivateDelay = 5;

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            StartCoroutine(DeactivateDelay());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IDamageable enemy))
            {
                enemy.Hitpoints -= bulletDamage;
                DeactivateAndEnqueue();
            }
        }

        private IEnumerator DeactivateDelay()
        {
            yield return new WaitForSeconds(deactivateDelay);
            DeactivateAndEnqueue();
        }

        private void DeactivateAndEnqueue()
        {
            BulletPool.GetInstance().Enqueue(this);
        }
    }
}