using UnityEngine;

namespace Shooter.Player
{
    public class Gravity : MonoBehaviour
    {
        [SerializeField]
        private Transform groundedTransform = default;
        [SerializeField]
        private LayerMask layersToDetect = default;
        [SerializeField]
        private float groundingCheckRadius = 0.125f;
        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Collider[] collidersHit = Physics.OverlapSphere(groundedTransform.position, groundingCheckRadius, layersToDetect);
            if (collidersHit.Length <= 0)
            {
                Debug.Log("grounded");
                // While player is not grounded, apply gravity to player.
                Vector3 gravity = new Vector3(0, Mathf.Abs(Physics.gravity.y / Mathf.Pow(transform.position.y, 2)), 0);
                characterController.SimpleMove(-gravity);
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundedTransform.position, groundingCheckRadius);
        }
        #endif
    }
}