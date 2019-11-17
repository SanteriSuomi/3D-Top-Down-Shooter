using UnityEngine;

namespace Shooter.Enemy
{
    public class EnemyShared : MonoBehaviour
    {
        public static Vector3 ObjectivePos { get; set; }
        public static LayerMask LayersToDetect { get; set; }
        public static float CheckRadius { get; set; } = 7.5f;
        public static float FundGiveAmount { get; set; } = 2;
        public static float DistanceCheckInterval { get; set; } = 0.25f;

        private void Awake()
        {
            ObjectivePos = FindObjectOfType<Player.Player>().transform.position;
            LayersToDetect = LayerMask.GetMask("Player");
        }
    }
}