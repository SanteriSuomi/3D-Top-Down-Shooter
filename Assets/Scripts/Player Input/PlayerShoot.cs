using UnityEngine;

namespace Shooter.Utility
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField]
        private Transform barrelEnd = default;
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
            shootTimer += Time.deltaTime;
            RaycastHit rayHit = ShootRaycast();
            if (shootTimer >= shootRate && rayHit.collider.TryGetComponent(out IDamageable _) && rayHit.collider.CompareTag(shootAbleTag))
            {
                shootTimer = 0;
                ShootBullet();
            }
        }

        private RaycastHit ShootRaycast()
        {
            Physics.Raycast(barrelEnd.position, barrelEnd.forward * shootDistance, out RaycastHit rayHit);
            return rayHit;
        }

        private void ShootBullet()
        {
            Bullet bullet = BulletPool.GetInstance().Dequeue();
            bullet.transform.position = barrelEnd.position;
            bullet.RigidBody.velocity = barrelEnd.forward * bulletSpeed;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(barrelEnd.position, barrelEnd.forward * shootDistance);
        }
        #endif
    }
}