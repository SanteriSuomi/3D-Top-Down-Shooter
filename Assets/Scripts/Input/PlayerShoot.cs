using UnityEngine;

namespace Shooter.Utility
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField]
        private Transform barrelEnd;
        [SerializeField]
        private LayerMask layersToHit;
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
            Physics.Raycast(barrelEnd.position, barrelEnd.forward * shootDistance, out RaycastHit rayHit, layersToHit);
            if (shootTimer >= shootRate && rayHit.collider)
            {
                Debug.Log($"Shooting");
                shootTimer = 0;
                Bullet bullet = BulletPool.GetInstance().Dequeue();
                bullet.transform.position = barrelEnd.position;
                bullet.GetComponent<Rigidbody>().velocity = barrelEnd.transform.forward * bulletSpeed;
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(barrelEnd.position, barrelEnd.forward * shootDistance);
        }
        #endif
    }
}