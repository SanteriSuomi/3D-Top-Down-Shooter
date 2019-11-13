using UnityEngine;

namespace Shooter.Utility
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField]
        private Transform barrelEnd = default;
        [SerializeField]
        private LayerMask layersToHit = default;
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
            if (shootTimer >= shootRate && rayHit.collider.TryGetComponent(out IDamageable enemy))
            {
                ShootBullet();
            }
        }

        private RaycastHit ShootRaycast()
        {
            Physics.Raycast(barrelEnd.position, barrelEnd.forward * shootDistance, out RaycastHit rayHit, layersToHit);
            return rayHit;
        }

        private void ShootBullet()
        {
            shootTimer = 0;
            Bullet bullet = BulletPool.GetInstance().Dequeue();
            bullet.transform.position = barrelEnd.position;
            bullet.GetComponent<Rigidbody>().velocity = barrelEnd.transform.forward * bulletSpeed;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(barrelEnd.position, barrelEnd.forward * shootDistance);
        }
        #endif
    }
}