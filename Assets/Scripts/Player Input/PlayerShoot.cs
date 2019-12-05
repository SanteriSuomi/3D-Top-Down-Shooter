using Photon.Pun;
using UnityEngine;

namespace Shooter.Utility
{
    public class PlayerShoot : MonoBehaviourPunCallbacks
    {
        public delegate void OnAttack(bool playAnim);
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
            shootTimer += Time.deltaTime;
            RaycastHit rayHit = ShootRaycast();
            if (shootTimer >= shootRate
                && rayHit.collider
                && rayHit.collider.CompareTag(shootAbleTag)
                && rayHit.collider.TryGetComponent(out IDamageable _))
            {
                shootTimer = 0;
                ShootBullet();
                ShootAnimation(true);
            }
            else
            {
                ShootAnimation(false);
            }
        }

        private RaycastHit ShootRaycast()
        {
            // Raycast forward from the player, and make sure to collide with trigger colliders.
            Physics.Raycast(barrelEnd.position, barrelEnd.forward, out RaycastHit rayHit, shootDistance, layersToDetect, QueryTriggerInteraction.Collide);
            return rayHit;
        }

        private void ShootBullet()
        {
            // De-pool the bullet and apply velocity to it.
            Bullet bullet = BulletPool.GetInstance().Dequeue();
            bullet.transform.position = barrelEnd.position;
            bullet.BulletRigidBody.velocity = barrelEnd.forward * bulletSpeed;
        }

        private void ShootAnimation(bool playAnim)
        {
            // Method that controls the shooting animation.
            OnAttackEvent.Invoke(playAnim);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(barrelEnd.position, barrelEnd.forward * shootDistance);
        }
        #endif
    }
}