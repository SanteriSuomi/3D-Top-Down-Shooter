using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.UI
{
    public class MenuScene : MonoBehaviour
    {
        [SerializeField] private GameObject m_VolumeButton = default;
        [SerializeField] private GameObject m_VolumeOffButton = default;
        [SerializeField] private GameObject m_Menu = default;
        [SerializeField] private GameObject m_MenuTwo = default;
        [SerializeField] private GameObject m_SettingsMenu = default;
        private readonly string levelSceneString = "SCE_Level";

        public void OnPlayClick()
        {
            m_Menu.SetActive(false);
            m_MenuTwo.SetActive(true);
            m_SettingsMenu.SetActive(false);
        }

        public void OnPlayClickNewGame()
        {
            LoadSettings(load: false);
            LoadScene();
        }

        public void OnPlayClickLoadGame()
        {
            LoadSettings(load: true);
            LoadScene();
        }

        private static void LoadSettings(bool load)
        {
            MenuSceneLoad.GetInstance().LoadPlayerSettings = load;
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(levelSceneString);
        }

        public void OnSettingsClick()
        {
            m_Menu.SetActive(false);
            m_MenuTwo.SetActive(false);
            m_SettingsMenu.SetActive(true);
        }

        public void OnQuitClick()
        {
            Application.Quit();
        }

        public void OnVolumeOnButtonClick()
        {
            AudioListener.volume = 0.0f;
            m_VolumeOffButton.SetActive(true);
            m_VolumeButton.SetActive(false);
        }

        public void OnVolumeOffButtonClick()
        {
            AudioListener.volume = 1f;
            m_VolumeButton.SetActive(true);
            m_VolumeOffButton.SetActive(false);
        }

        public void OnReturnToMenuButtonClick()
        {
            m_Menu.SetActive(true);
            m_MenuTwo.SetActive(false);
            m_SettingsMenu.SetActive(false);
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
        }
    }
}