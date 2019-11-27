using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerAudioController : MonoBehaviour
    {
        private PlayerShoot playerShoot;
        private AudioSource[] audioSource;
        private CharacterController characterController;

        private void Awake()
        {
            playerShoot = GetComponent<PlayerShoot>();
            playerShoot.OnAttackEvent += OnAttack;
            audioSource = GetComponents<AudioSource>();
            characterController = GetComponent<CharacterController>();
        }

        private void OnAttack(bool playAnim)
        {
            // Play the attack audio only when it isn't playing and it's activated by invoking the event.
            if (!audioSource[0].isPlaying && playAnim)
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
            if (characterController.velocity.sqrMagnitude > 0 && !audioSource[1].isPlaying)
            {
                // If player's velocity magnitude is more than 0, footstep sound has to be played.
                audioSource[1].Play();
            }
        }

        private void OnDisable()
        {
            playerShoot.OnAttackEvent -= OnAttack;
        }
    }
}