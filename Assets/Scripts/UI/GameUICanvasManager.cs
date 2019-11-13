using Shooter.Utility;
using UnityEngine;

namespace Shooter.UI
{
    public class GameUICanvasManager : GenericSingleton<GameUICanvasManager>
    {
        [SerializeField]
        private GameObject followerShopButton = default;

        public void FollowerShopButton()
        {
            followerShopButton.SetActive(!followerShopButton.activeSelf);
        }
    }
}