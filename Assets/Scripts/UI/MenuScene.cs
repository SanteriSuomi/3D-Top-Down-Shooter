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
        private CanvasGroup m_FadeGroup;
        private readonly float m_FadeInSpeed = 0.33f;

        private void Start()
        {
            m_FadeGroup = FindObjectOfType<CanvasGroup>();
            m_FadeGroup.alpha = 1;
        }

        private void Update()
        {
            m_FadeGroup.alpha = 1 - Time.timeSinceLevelLoad * m_FadeInSpeed;
        }

        public void OnPlayClick()
        {
            m_Menu.SetActive(false);
            m_MenuTwo.SetActive(true);
            m_SettingsMenu.SetActive(false);
        }

        public void OnPlayClickNewGame()
        {
            MenuSceneLoad.GetInstance().LoadPlayerSettings = false;
            SceneManager.LoadScene("SCE_Level");
        }

        public void OnPlayClickLoadGame()
        {
            MenuSceneLoad.GetInstance().LoadPlayerSettings = true;
            SceneManager.LoadScene("SCE_Level");
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