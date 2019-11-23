using Shooter.AI;
using Shooter.Player;
using Shooter.Shop;
using Shooter.Utility;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class ShopSpawner : GenericSingleton<ShopSpawner>
    {
        public int AmountOfFollowers { get; set; } = 0;
        [SerializeField]
        private TextMeshProUGUI fundsOutText = default;
        private Transform player;
        private WaitForSeconds fundsOut;
        [SerializeField]
        private string maxFollowersAchieved = "Maximum amount of followers achieved";
        [SerializeField]
        private string notEnoughFunds = "Not enough funds";
        [SerializeField]
        private float fundsOutTextTimer = 2;
        [SerializeField]
        private float shopObjectSpawnRange = 4;
        [SerializeField]
        private int maxAmountOfFollowers = 3;
        private bool isFundsCoroutineRunning;

        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<Player.Player>().transform;
            fundsOut = new WaitForSeconds(fundsOutTextTimer);
        }

        public void SpawnObject(ShopObject shopObject)
        {
            if (AmountOfFollowers == maxAmountOfFollowers || shopObject.Cost > PlayerSettings.GetInstance().Funds)
            {
                if (!isFundsCoroutineRunning)
                {
                    isFundsCoroutineRunning = true;
                    BuyFailedPopup(shopObject);
                }

                return;
            }

            Initialize(shopObject);
        }

        private void BuyFailedPopup(ShopObject shopObject)
        {
            Follower follower = shopObject.Prefab.GetComponent<Follower>();
            if (follower != null)
            {
                StartCoroutine(FundsOutText(maxFollowersAchieved));
                AmountOfFollowers++;
            }
            else
            {
                StartCoroutine(FundsOutText(notEnoughFunds));
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

        private IEnumerator FundsOutText(string text)
        {
            fundsOutText.text = text;
            fundsOutText.gameObject.SetActive(true);
            yield return fundsOut;
            fundsOutText.gameObject.SetActive(false);
            isFundsCoroutineRunning = false;
        }
    }
}