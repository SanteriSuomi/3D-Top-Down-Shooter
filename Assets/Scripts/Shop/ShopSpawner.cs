using Shooter.AI;
using Shooter.Player;
using Shooter.Shop;
using Shooter.Utility;
using UnityEngine;

namespace Shooter.UI
{
    public class ShopSpawner : GenericSingleton<ShopSpawner>
    {
        public int AmountOfFollowers { get; set; } = 0;
        private Transform player;
        [SerializeField]
        private string maxFollowersAchieved = "Maximum amount of followers achieved";
        [SerializeField]
        private string notEnoughFunds = "Not enough funds";
        [SerializeField]
        private float shopObjectSpawnRange = 4;
        [SerializeField]
        private int maxAmountOfFollowers = 3;
        private bool isFundsCoroutineRunning;

        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<Player.Player>().transform;
        }

        public void SpawnObject(ShopObject shopObject)
        {
            if (AmountOfFollowers >= maxAmountOfFollowers || shopObject.Cost > PlayerSettings.GetInstance().Funds)
            {
                if (!isFundsCoroutineRunning)
                {
                    isFundsCoroutineRunning = true;
                    BuyFailedPopup(shopObject);
                }
            }
            else
            {
                Initialize(shopObject);
            }
        }

        private void BuyFailedPopup(ShopObject shopObject)
        {
            if (shopObject.Prefab.TryGetComponent(out Follower _))
            {
                NoFundsEventHandler.TriggerFundsOutPopUp(maxFollowersAchieved);
                AmountOfFollowers++;
            }
            else
            {
                NoFundsEventHandler.TriggerFundsOutPopUp(notEnoughFunds);
            }
        }

        private void Initialize(ShopObject shopObject)
        {
            PlayerSettings.GetInstance().Funds -= shopObject.Cost;
            GameObject spawnedObject = Instantiate(shopObject.Prefab);
            Vector3 spawnPos = player.position + new Vector3(Random.Range(-shopObjectSpawnRange, shopObjectSpawnRange),
                0, Random.Range(-shopObjectSpawnRange, shopObjectSpawnRange));
            spawnedObject.transform.position = spawnPos;
        }
    }
}