using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private PlayerShoot playerShoot;
        private CharacterController characterController;
        private Animator animator;
        [SerializeField]
        private float animationRotationSpeed = 7.5f;

        private void Awake()
        {
            playerShoot = GetComponent<PlayerShoot>();
            playerShoot.OnAttackEvent += OnAttack;
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        private void OnAttack(bool playAnim)
        {
            // Update the animation parameter accordingly.
            animator.SetBool("shoot", playAnim);
        }

        private void Update()
        {
            AnimRotation();
        }

        private void AnimRotation()
        {
            // Animation rotation should only be update while player is moving.
            if (characterController.velocity.sqrMagnitude > 0)
            {
                // Get the character velocity and store it.
                float velocityX = characterController.velocity.x;
                float velocityY = characterController.velocity.z;
                SetAnimVelocity(new Vector2(velocityX, velocityY));

                // Character direction is velocity normalized.
                Vector3 direction = characterController.velocity.normalized;
                // Character is not supposed to move up.
                direction.y = 0;
                // Make sure direction is not zero.
                if (direction.sqrMagnitude > 0)
                {
                    // Update the rotation to the direction character is moving to.
                    Quaternion lookDirection = Quaternion.LookRotation(direction, Vector2.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, animationRotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                // If character is not moving, make sure to disable the movement animations.
                SetAnimVelocity(new Vector2(0, 0));
            }
        }

        private void SetAnimVelocity(Vector2 velocity)
        {
            // Update the animation blend tree parameters with the velocity.
            animator.SetFloat("velocityX", velocity.x);
            animator.SetFloat("velocityY", velocity.y);
        }

        private void OnDisable()
        {
            playerShoot.OnAttackEvent -= OnAttack;
        }
    }
}