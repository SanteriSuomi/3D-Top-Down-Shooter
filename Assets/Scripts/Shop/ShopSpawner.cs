using Shooter.Player;
using Shooter.Shop;
using Shooter.Utility;
using UnityEngine;

namespace Shooter.UI
{
    public class ShopSpawner : GenericSingleton<ShopSpawner>
    {
        public int AmountOfFollowers { get; set; } = 0;
        [SerializeField]
        private string maxFollowersAchieved = "Maximum amount of followers achieved";
        [SerializeField]
        private string notEnoughFunds = "Not enough funds";
        [SerializeField]
        private float shopObjectSpawnRange = 4;
        [SerializeField]
        private int maxAmountOfFollowers = 3;

        public void SpawnObject(ShopObject shopObject)
        {
            if (shopObject.Cost > PlayerSettings.GetInstance().Funds)
            {
                NoFundsEventHandler.TriggerFundsOutPopUp(notEnoughFunds);
            }
            else if (AmountOfFollowers >= maxAmountOfFollowers && shopObject.Prefab.TryGetComponent(out IShopSpawnable _))
            {
                NoFundsEventHandler.TriggerFundsOutPopUp(maxFollowersAchieved);
            }
            else
            {
                Initialize(shopObject);
            }
        }

        private void Initialize(ShopObject shopObject)
        {
            CheckForSpawnable(shopObject, out IShopSpawnable spawnableObject);
            // Deduct funds from player.
            PlayerSettings.GetInstance().Funds -= shopObject.Cost;
            if (spawnableObject != null)
            {
                AmountOfFollowers++;
                // Instantiate the object and set it's position.
                spawnableObject.SpawnItem(spawnRange: shopObjectSpawnRange);
            }
        }

        private void CheckForSpawnable(ShopObject shopObject, out IShopSpawnable spawnableObject)
        {
            // Check if this item is spawnable by getting the interface component from it.
            spawnableObject = shopObject.Prefab.GetComponent<IShopSpawnable>();
        }
    }
}