using UnityEngine;

namespace Shooter.Enemy
{
    public class EnemySharedData : MonoBehaviour
    {
        public static Transform Objective { get; set; }

        private void Awake()
        {
            Objective = FindObjectOfType<Player.Player>().transform;
        }
    }
}