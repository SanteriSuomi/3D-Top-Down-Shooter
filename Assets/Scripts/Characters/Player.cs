using Shooter.AI;
using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.Player
{
    public class Player : Character
    {
        public static float Funds { get; set; }
        [SerializeField]
        private float initialFunds = 10;
        public static float PlayerSensitivityMultiplier { get; set; }
        [SerializeField]
        private float initialPlayerSensitivityMultiplier = 1;

        protected override void InitializeState()
        {
            Funds = initialFunds;
            PlayerSensitivityMultiplier = initialPlayerSensitivityMultiplier;
        }

        protected override void StartState()
        {
            
        }

        protected override void UpdateState()
        {
            
        }

        public void LoadPlayer()
        {
            PlayerData data = SaveSystem.LoadPlayer();
            Vector3 position;
            position.x = data.GetPosition()[0];
            position.y = data.GetPosition()[1];
            position.z = data.GetPosition()[2];
            transform.position = position;

            Vector3 rotation;
            rotation.x = data.GetRotation()[0];
            rotation.y = data.GetRotation()[1];
            rotation.z = data.GetRotation()[2];
            transform.rotation = Quaternion.Euler(rotation);
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                SavePlayer();
            }
        }

        private void SavePlayer()
        {
            SaveSystem.SavePlayer(this);
        }

        protected override void OnZeroHP()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}