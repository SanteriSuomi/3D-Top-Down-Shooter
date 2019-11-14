using Shooter.Player;
using Shooter.Shop;
using Shooter.Utility;

namespace Shooter.UI
{
    public class ShopSpawner : GenericSingleton<ShopSpawner>
    {
        public void Spawn(ShopObject objectToSpawn)
        {
            if (PlayerSettings.GetInstance().Funds >= objectToSpawn.Cost)
            {

            }
        }
    }
}