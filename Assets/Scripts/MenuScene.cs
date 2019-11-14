using Shooter.Player;
using Shooter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : GenericSingleton<MenuScene>
{
	private CanvasGroup m_FadeGroup;
	private float m_FadeInSpeed = 0.33f;
	[SerializeField] private GameObject m_VolumeButton = default;
	[SerializeField] private GameObject m_VolumeOffButton = default;
	[SerializeField] public GameObject m_Menu = default;
	public GameObject m_MenuTwo;
	public GameObject m_SettingsMenu;
    private bool loadPlayer;

    private void Start()
	{
		m_FadeGroup = FindObjectOfType<CanvasGroup>();
		m_FadeGroup.alpha = 1;
	}

	private void Update()
	{
		m_FadeGroup.alpha = 1 - Time.timeSinceLevelLoad * m_FadeInSpeed;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SCE_Level" && loadPlayer)
        {
            Debug.Log($"{scene.name} loaded");
            FindObjectOfType<Player>().GetComponent<PlayerSettings>().LoadPlayer();
        }
    }

    public void OnPlayClick()
	{
		m_Menu.SetActive(false);
		m_MenuTwo.SetActive(true);
		m_SettingsMenu.SetActive(false);
	}

	public void OnPlayClickNewGame()
	{
        loadPlayer = false;
		SceneManager.LoadScene("SCE_Level");
	}

    public void OnPlayClickLoadGame()
    {
        loadPlayer = true;
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
