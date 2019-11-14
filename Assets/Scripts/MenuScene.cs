using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
	private CanvasGroup m_FadeGroup;
	private float m_FadeInSpeed = 0.33f;
	[SerializeField] private GameObject m_VolumeButton;
	[SerializeField] private GameObject m_VolumeOffButton;
	[SerializeField] public GameObject m_Menu;
	public GameObject m_MenuTwo;
	public GameObject m_SettingsMenu;
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
	public void OnPlayClickMenu2()
	{
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
		Debug.Log(volume);
	}
}
