using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.UI
{
    // Disable "mark as static errors, as these events are accessed by unity events.
    #pragma warning disable CA1822
    public class MenuScene : GenericSingleton<MenuScene>
    {
        //
        // MenuScene controls the main menu.
        //
        [SerializeField]
        private GameObject volumeOnButton = default;
        [SerializeField]
        private GameObject volumeOffButton = default;
        [SerializeField]
        private GameObject menu = default;
        [SerializeField]
        private GameObject menuTwo = default;
        [SerializeField]
        private GameObject settingsMenu = default;

        public void OnPlayClick()
        {
            menu.SetActive(false);
            menuTwo.SetActive(true);
            settingsMenu.SetActive(false);
        }

        public void OnPlayClickNewGame()
        {
            // Signal that settings cannot be loaded.
            LoadSettings(load: false);
            LoadScene();
        }

        public void OnPlayClickLoadGame()
        {
            // Signal that settings can be loaded.
            LoadSettings(load: true);
            LoadScene();
        }

        private void LoadSettings(bool load)
        {
            // Applies whether or not player settings should be loaded.
            MenuSceneLoad.GetInstance().LoadPlayerSettings = load;
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(LevelShared.levelSceneString);
        }

        public void OnSettingsClick()
        {
            menu.SetActive(false);
            menuTwo.SetActive(false);
            settingsMenu.SetActive(true);
        }

        public void OnQuitClick()
        {
            Application.Quit();
        }

        public void OnVolumeOnButtonClick()
        {
            AudioListener.volume = 0;
            volumeOffButton.SetActive(true);
            volumeOnButton.SetActive(false);
        }

        public void OnVolumeOffButtonClick()
        {
            AudioListener.volume = 1;
            volumeOnButton.SetActive(true);
            volumeOffButton.SetActive(false);
        }

        public void OnReturnToMenuButtonClick()
        {
            menu.SetActive(true);
            menuTwo.SetActive(false);
            settingsMenu.SetActive(false);
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
        }
    }
}