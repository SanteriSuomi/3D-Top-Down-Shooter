using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class AnimationController : MonoBehaviour
    {
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

        private void OnAttack(float animFloat)
        {
            animator.SetFloat("Attacking", animFloat);
        }

        private void Update()
        {
            Walk();
        }

        private void Walk()
        {
            if (characterController.velocity.sqrMagnitude >= 0.1f)
            {
                animator.SetFloat("Walking", 1);
            }
            else
            {
                animator.SetFloat("Walking", 0);
            }
        }

        private void OnDisable()
        {
            playerShoot.OnAttackEvent -= OnAttack;
        }
    }
}