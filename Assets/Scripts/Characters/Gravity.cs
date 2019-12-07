using UnityEngine;

namespace Shooter.Player
{
    public class Gravity : MonoBehaviour
    {
        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (!characterController.isGrounded)
            {
                Vector3 gravity = new Vector3(0, Physics.gravity.y / Mathf.Pow(transform.position.y, 2), 0);
                characterController.attachedRigidbody.AddForce(gravity);
            }
        }
    }
}