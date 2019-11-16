using UnityEngine;

namespace Shooter.Shop
{
    [CreateAssetMenu(fileName = "Shop Object", menuName = "ScriptableObjects/Shop Object", order = 1)]
    public class ShopObject : ScriptableObject
    {
        [SerializeField]
        private float cost = default;
        public float Cost  { get { return cost; } }
        [SerializeField]
        private new string name = default;
        public string Name { get { return name; } }
        [SerializeField]
        private GameObject prefab = default;
        public GameObject Prefab  { get { return prefab; } }
    }
}