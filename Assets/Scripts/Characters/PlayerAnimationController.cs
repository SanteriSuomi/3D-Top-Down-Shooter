using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField]
        private float animationRotationSpeed = 7.5f;
        private PlayerShoot playerShoot;
        private CharacterController characterController;
        private Animator animator;

        private void Awake()
        {
            playerShoot = GetComponent<PlayerShoot>();
            playerShoot.OnAttackEvent += OnAttack;
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        private void OnAttack(bool playAnim)
        {
            animator.SetBool("shoot", playAnim);
        }

        private void Update()
        {
            AnimRotation();
        }

        private void AnimRotation()
        {
            if (characterController.velocity.sqrMagnitude > 0)
            {
                float velocityX = characterController.velocity.x;
                float velocityY = characterController.velocity.z;
                SetAnimVelocity(new Vector2(velocityX, velocityY));

                Vector3 direction = characterController.velocity.normalized;
                direction.y = 0;
                if (direction.sqrMagnitude > 0)
                {
                    Quaternion lookDirection = Quaternion.LookRotation(direction, Vector2.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, animationRotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                SetAnimVelocity(new Vector2(0, 0));
            }
        }

        private void SetAnimVelocity(Vector2 velocity)
        {
            animator.SetFloat("velocityX", velocity.x);
            animator.SetFloat("velocityY", velocity.y);
        }

        private void OnDisable()
        {
            playerShoot.OnAttackEvent -= OnAttack;
        }
    }
}