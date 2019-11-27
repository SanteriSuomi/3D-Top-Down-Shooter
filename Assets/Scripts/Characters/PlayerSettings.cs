using Shooter.Utility;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerSettings : GenericSingleton<PlayerSettings>
    {
        // Delegates and events invoked when according values get updated here.
        public delegate void OnHitpointChange(float hitPoints);
        public event OnHitpointChange OnHitpointChangeEvent;

        public delegate void OnFundsChange(float funds);
        public event OnFundsChange OnFundsChangeEvent;

        public delegate void OnScoreChange(float score);
        public event OnScoreChange OnScoreChangeEvent;

        [SerializeField]
        private int autoSaveIntervalSeconds = 60;

        private float hitPoints = 100;
        public float HitPoints
        {
            get { return hitPoints; }
            set
            {
                hitPoints = value;
                // Update all the other places that use player hitpoints when hitpoints get updated.
                OnHitpointChangeEvent.Invoke(hitPoints);
            }
        }

        private float funds = 10;
        public float Funds
        {
            get { return funds; }
            set
            {
                funds = value;
                OnFundsChangeEvent.Invoke(funds);
            }
        }

        private float score = 0;
        public float Score
        {
            get { return score; }
            set
            {
                score = value;
                OnScoreChangeEvent.Invoke(score);
            }
        }
        // Sensitivity multiplier is used from main menu sensitivity settings, and used to for rotation sensitivity.
        public float PlayerSensitivityMultiplier { get; set; }

        private void Start()
        {
            // Sync all the values that use there value here initially.
            OnHitpointChangeEvent?.Invoke(hitPoints);
            OnFundsChangeEvent?.Invoke(funds);
            OnScoreChangeEvent?.Invoke(score);
        }

        private void Update()
        {
            // Autosave is done every X second interval from the level load.
            if (Mathf.RoundToInt(Time.timeSinceLevelLoad) % autoSaveIntervalSeconds == 0)
            {
                SavePlayer();
            }
        }

        public void LoadPlayer()
        {
            // Use the savesystem class for deserializing the playersavedata and update the the player values (on load).
            PlayerSaveData data = SaveSystem.LoadPlayer();

            HitPoints = data.Hitpoints;
            Funds = data.Funds;
            Score = data.Score;
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
        // Since on OnApplicationQuit does not work correctly on mobile, use the OnApplicationPause method for android.
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
            SavePlayer();
        }
        #endif

        private void SavePlayer()
        {
            SaveSystem.SavePlayer(this);
        }
    }
}