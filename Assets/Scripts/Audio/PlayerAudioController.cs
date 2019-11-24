using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerAudioController : MonoBehaviour
    {
        PlayerShoot playerShoot;
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
                audioSource[1].Play();
            }
        }

        private void OnDisable()
        {
            playerShoot.OnAttackEvent -= OnAttack;
        }
    }
}