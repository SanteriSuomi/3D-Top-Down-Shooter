using Shooter.AI;
using Shooter.Player;
using Shooter.Shop;
using Shooter.Utility;
using UnityEngine;

namespace Shooter.UI
{
    public class ShopSpawner : GenericSingleton<ShopSpawner>
    {
        private Transform player;

        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<Player.Player>().transform;
        }

        public void SpawnFollower(ShopObject objectToSpawn)
        {
            if (objectToSpawn.TryGetComponent(out ShopObject shopObject))
            {
                float availableFunds = PlayerSettings.GetInstance().Funds;
                if (shopObject.Cost > availableFunds)
                {
                    return;
                }

                PlayerSettings.GetInstance().Funds -= shopObject.Cost;
                Follower follower = FollowerPool.GetInstance().Dequeue();
                follower.transform.position = player.position + new Vector3(Random.Range(2, 5), 0, 0);
            }
        }
    }
}