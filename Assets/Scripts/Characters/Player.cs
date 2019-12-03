using Shooter.AI;
using Shooter.Network;
using UnityEngine;
using UnityEngine.Assertions;

namespace Shooter.Player
{
    public class Player : Character
    {
        private PlayerSettings playerSettings;

        protected override void InitializeState()
        {
            playerSettings = GetComponent<PlayerSettings>();
            playerSettings.OnHitpointChangeEvent += OnHitpointsChange;
        }

        private void OnHitpointsChange(float hitPoints)
        {
            HitPoints = hitPoints;
        }

        protected override void StartState()
        {
            if (!photonView.IsMine)
            {
                // Warn if this isn't a local player.
                #if UNITY_EDITOR
                Debug.LogWarning("This player is not local player");
                #endif
            }

            HitPoints = startingHitPoints;
        }

        protected override void UpdateState()
        {
            // Empty on purpose.
        }

        protected override void OnTakeDamage(float damage)
        {
            base.OnTakeDamage(damage);
            playerSettings.HitPoints -= damage;
        }

        protected override void OnZeroHP()
        {
            #if UNITY_STANDALONE
            // Make sure the cursor is enabled for the menu (only on computer builds).
            Cursor.visible = true;
            #endif

            #if UNITY_EDITOR
            // When player dies they will get back to the main menu or "lobby".
            Assert.IsNotNull(NetworkGameManager.GetInstance());
            #endif

            NetworkGameManager.GetInstance().LeaveRoomAndDisconnect();
        }

        public override void OnDisable()
        {
            playerSettings.OnHitpointChangeEvent -= OnHitpointsChange;
        }
    }
}