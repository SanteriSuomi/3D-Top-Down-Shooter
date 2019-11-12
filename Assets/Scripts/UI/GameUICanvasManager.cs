using Shooter.Utility;
using UnityEngine;

namespace Shooter.UI
{
    public class GameUICanvasManager : Singleton<GameUICanvasManager>
    {
        [SerializeField]
        private GameObject followerShopButton = default;

        public void FollowerShopButton()
        {
            followerShopButton.SetActive(!gameObject.activeSelf);
        }
    }
}