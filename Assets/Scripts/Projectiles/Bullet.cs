using System.Collections;
using UnityEngine;

namespace Shooter.Utility
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float deactivateDelay = 5;
        [SerializeField]
        private LayerMask layersToHit;

        private void OnEnable()
        {
            StartCoroutine(DeactivateDelay());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == layersToHit.value)
            {
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