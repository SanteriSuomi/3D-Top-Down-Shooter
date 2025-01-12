﻿using Shooter.Utility;
using UnityEngine;

namespace Shooter.UI
{
    public class ShopCanvasManager : GenericSingleton<ShopCanvasManager>
    {
        //
        // ShopCanvasManager manages activating/disabling shop buttons.
        //
        [SerializeField]
        private GameObject followerShopButton = default;
        [SerializeField]
        private GameObject healthPotionShopButton = default;

        public void FollowerShopButton()
        {
            followerShopButton.SetActive(!followerShopButton.activeSelf);
            healthPotionShopButton.SetActive(!healthPotionShopButton.activeSelf);
        }
    }
}