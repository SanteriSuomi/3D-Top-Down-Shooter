using System.Collections;
using UnityEngine;

namespace Shooter.Utility
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float deactivateDelay = 5;

        private void OnEnable()
        {
            StartCoroutine(DeactivateDelay());
        }

        private IEnumerator DeactivateDelay()
        {
            yield return new WaitForSeconds(deactivateDelay);
            BulletPool.GetInstance().Enqueue(this);
        }
    }
}