using Shooter.Player;
using Shooter.Utility;
using UnityEngine.SceneManagement;

namespace Shooter.UI
{
    public class MenuSceneLoad : GenericSingleton<MenuSceneLoad>
    {
        public bool LoadPlayerSettings { get; set; }

        protected override void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "SCE_Level" && LoadPlayerSettings)
            {
                PlayerSettings.GetInstance().LoadPlayer();
            }
        }
    }
}