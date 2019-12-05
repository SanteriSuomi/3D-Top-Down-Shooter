using UnityEngine;

namespace Shooter.Shop
{
    [CreateAssetMenu(fileName = "Shop Text", menuName = "ScriptableObjects/New Shop Text", order = 3)]
    public class ShopText : ScriptableObject
    {
        [SerializeField]
        private string fundsOut = "Not enough funds";
        public string FundsOut { get { return fundsOut; } }
        [SerializeField]
        private string maxFollowersAchieved = "Max Followers Achieved";
        public string MaxFollowersAchieved { get { return maxFollowersAchieved; } }
    }
}