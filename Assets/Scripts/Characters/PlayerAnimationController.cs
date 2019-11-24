using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerAnimationController : MonoBehaviour
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
            if (characterController.velocity.sqrMagnitude > 0)
            {
                OnWalk(1);
            }
            else
            {
                OnWalk(0);
            }
        }

        private void OnWalk(float animFloat)
        {
            animator.SetFloat("Walking", animFloat);
        }

        private void OnDisable()
        {
            playerShoot.OnAttackEvent -= OnAttack;
        }
    }
}