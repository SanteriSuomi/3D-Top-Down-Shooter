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
        [SerializeField]
        private TextMeshProUGUI fundsOutText = default;
        private Transform player;
        private WaitForSeconds fundsOut;
        [SerializeField]
        private float fundsOutTextTimer = 2;
        private bool isFundsCoroutineRunning;
        [SerializeField]
        private int maxAmountOfFollowers = 3;
        private int amountOfFollowers = 0;
        [SerializeField]
        private string maxFollowersAchieved = "Maximum amount of followers achieved";
        [SerializeField]
        private string notEnoughFunds = "Not enough funds";
        [SerializeField]
        private float shopObjectSpawnRange = 4;

        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<Player.Player>().transform;
            fundsOut = new WaitForSeconds(fundsOutTextTimer);
        }

        public void SpawnObject(ShopObject shopObject)
        {
            if (shopObject.Prefab.TryGetComponent(out Follower follower) && amountOfFollowers == maxAmountOfFollowers || shopObject.Cost > PlayerSettings.GetInstance().Funds)
            {
                if (!isFundsCoroutineRunning)
                {
                    isFundsCoroutineRunning = true;
                    if (follower != null)
                    {
                        StartCoroutine(FundsOutText(maxFollowersAchieved));
                    }
                    else
                    {
                        StartCoroutine(FundsOutText(notEnoughFunds));
                    }
                    #if UNITY_EDITOR
                    Debug.Log($"{shopObject.Prefab.name} couldn't be purchased.");
                    #endif
                }

                return;
            }

            if (follower != null)
            {
                amountOfFollowers++;
            }

            InstantiateObject(shopObject);
        }

        private void InstantiateObject(ShopObject shopObject)
        {
            PlayerSettings.GetInstance().Funds -= shopObject.Cost;
            GameObject spawnedObject = Instantiate(shopObject.Prefab);
            spawnedObject.transform.position = player.position + new Vector3(Random.Range(-shopObjectSpawnRange, shopObjectSpawnRange), 0, 0);
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