using Shooter.Player;
using Shooter.Utility;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class MenuSceneLoad : GenericSingleton<MenuSceneLoad>
    {
        public bool LoadPlayerSettings { get; set; }
        private float sensitivitySliderValue = 1;
        private readonly string levelSceneString = "SCE_Level";

        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        public void OnSensitivitySliderValueChanged(Slider slider)
        {
            // Dynamically update this sensitivity field with the value from sensitivity slider in menus.
            sensitivitySliderValue = slider.value;
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == levelSceneString && LoadPlayerSettings)
            {
                // If user pressed load button, update the sensitivity and load the player.
                UpdateSensitivity();
                PlayerSettings.GetInstance().LoadPlayer();
            }
            else if (scene.name == levelSceneString)
            {
                // Otherwise only update sensitivity.
                UpdateSensitivity();
            }
        }

        private void UpdateSensitivity()
        {
            PlayerSettings.GetInstance().PlayerSensitivityMultiplier = sensitivitySliderValue;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
    }
}