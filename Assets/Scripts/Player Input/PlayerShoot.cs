using UnityEngine;

namespace Shooter.Utility
{
    public class PlayerShoot : MonoBehaviour
    {
        public delegate void OnAttack(float animFloat);
        public event OnAttack OnAttackEvent;

        [SerializeField]
        private Transform barrelEnd = default;
        [SerializeField]
        private LayerMask layersToDetect = default;
        [SerializeField]
        private string shootAbleTag = "Enemy";
        [SerializeField]
        private float shootDistance = 15;
        [SerializeField]
        private float bulletSpeed = 15;
        [SerializeField]
        private float shootRate = 1;
        private float shootTimer;

        private void Update()
        {
            Shoot();
        }

        private void Shoot()
        {
            shootTimer += Time.deltaTime;
            RaycastHit rayHit = ShootRaycast();
            if (shootTimer >= shootRate
                && rayHit.collider.TryGetComponent(out IDamageable _)
                && rayHit.collider.CompareTag(shootAbleTag))
            {
                shootTimer = 0;
                ShootBullet();
                ShootAnimation(1);
            }
            else
            {
                ShootAnimation(0);
            }
        }

        private void ShootAnimation(float animFloat)
        {
            OnAttackEvent.Invoke(animFloat);
        }

        private RaycastHit ShootRaycast()
        {
            Physics.Raycast(barrelEnd.position, barrelEnd.forward, out RaycastHit rayHit, shootDistance, layersToDetect, QueryTriggerInteraction.Collide);
            return rayHit;
        }

        private void ShootBullet()
        {
            Bullet bullet = BulletPool.GetInstance().Dequeue();
            bullet.transform.position = barrelEnd.position;
            bullet.BulletRigidBody.velocity = barrelEnd.forward * bulletSpeed;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(barrelEnd.position, barrelEnd.forward * shootDistance);
        }
        #endif
    }
}