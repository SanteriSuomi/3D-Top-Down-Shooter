using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerSettings : GenericSingleton<PlayerSettings>
    {
        public delegate void OnHitpointChange(float hitPoints);
        public event OnHitpointChange OnHitpointChangeEvent;

        public delegate void OnFundsChange(float funds);
        public event OnFundsChange OnFundsChangeEvent;

        private float hitPoints = 100;
        public float HitPoints
        {
            get { return hitPoints; }
            set
            {
                hitPoints = value;
                OnHitpointChangeEvent.Invoke(hitPoints);
            }
        }

        private float funds = 100;
        public float Funds
        {
            get { return funds; }
            set
            {
                funds = value;
                OnFundsChangeEvent.Invoke(funds);
            }
        }

        public float PlayerSensitivityMultiplier { get; set; }

        private void Start()
        {
            OnHitpointChangeEvent?.Invoke(hitPoints);
            OnFundsChangeEvent?.Invoke(funds);
        }

        public void LoadPlayer()
        {
            PlayerSaveData data = SaveSystem.LoadPlayer();

            HitPoints = data.Hitpoints;
            Funds = data.Funds;
            PlayerSensitivityMultiplier = data.PlayerSensitivityMultiplier;

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

        #if UNITY_ANDROID
        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                SavePlayer();
            }
        }
        #endif

        #if UNITY_STANDALONE
        private void OnApplicationQuit()
        {
            SaveSystem.SavePlayer(this);
        }
        #endif

        private void SavePlayer()
        {
            SaveSystem.SavePlayer(this);
        }
    }
}