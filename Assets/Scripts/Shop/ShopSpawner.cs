using Shooter.Player;
using Shooter.Shop;
using Shooter.Utility;
using System.Collections;
using UnityEngine;

namespace Shooter.UI
{
    public class ShopSpawner : GenericSingleton<ShopSpawner>
    {
        [SerializeField]
        private GameObject fundsOutText = default;
        private Transform player;
        private bool isFundsCoroutineRunning;

        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<Player.Player>().transform;
        }

        public void SpawnObject(ShopObject shopObject)
        {
            if (shopObject.Cost > PlayerSettings.GetInstance().Funds)
            {
                if (!isFundsCoroutineRunning)
                {
                    isFundsCoroutineRunning = true;
                    StartCoroutine(FundsOutText());
                }
                #if UNITY_EDITOR
                Debug.Log($"{shopObject.Name} cost it too high.");
                #endif
                return;
            }

            InstantiateObject(shopObject);
        }

        private void InstantiateObject(ShopObject shopObject)
        {
            PlayerSettings.GetInstance().Funds -= shopObject.Cost;
            GameObject spawnedObject = Instantiate(shopObject.Prefab);
            spawnedObject.transform.position = player.position + new Vector3(Random.Range(-5, 5), 0, 0);
        }

        private IEnumerator FundsOutText()
        {
            fundsOutText.SetActive(true);
            yield return new WaitForSeconds(2);
            fundsOutText.SetActive(false);
            isFundsCoroutineRunning = false;
        }
    }
}