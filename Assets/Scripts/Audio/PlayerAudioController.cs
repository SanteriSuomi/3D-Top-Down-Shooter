using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerAudioController : MonoBehaviour
    {
        private AudioSource[] audioSource;
        private CharacterController characterController;

        private void Awake()
        {
            PlayerShoot playerShoot = GetComponent<PlayerShoot>();
            playerShoot.OnAttackEvent += OnAttack;
            audioSource = GetComponents<AudioSource>();
            characterController = GetComponent<CharacterController>();
        }

        private void OnAttack(float animFloat)
        {
            if (!audioSource[0].isPlaying && animFloat > 0)
            {
                audioSource[0].Play();
            }
        }

        private void Update()
        {
            FootStepSound();
        }

        private void FootStepSound()
        {
            if (characterController.velocity.sqrMagnitude > 0.1f && !audioSource[1].isPlaying)
            {
                audioSource[1].Play();
            }
        }
    }
}