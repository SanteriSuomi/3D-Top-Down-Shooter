using Shooter.Player;
using UnityEngine;

namespace Shooter.AI
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/Enemy Data", order = 2)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField]
        private Vector3 objectivePosition = new Vector3(0, 0, 0);
        public Vector3 ObjectivePosition { get { return objectivePosition; } }
        public Objective Objective { get; private set; }
        [SerializeField]
        private LayerMask layersToDetect = default;
        public LayerMask LayersToDetect { get { return layersToDetect; } }
        [SerializeField]
        private float checkRadius = 9;
        public float CheckRadius { get { return checkRadius; } }
        [SerializeField]
        private float damageDistance = 1;
        public float DamageDistance { get { return damageDistance; } }
        [SerializeField]
        private float dotProductMax = 0.25f;
        public float DotProductMax { get { return dotProductMax; } }
        [SerializeField]
        private float fundGiveAmount = 2;
        public float FundGiveAmount { get { return fundGiveAmount; } }
        [SerializeField]
        private float damageAmount = 5;
        public float DamageAmount { get { return damageAmount; } }
        [SerializeField]
        private float distanceCheckInterval = 0.25f;
        public float DistanceCheckInterval { get { return distanceCheckInterval; } }
        [SerializeField]
        private float pathUpdateInterval = 0.25f;
        public float PathUpdateInterval { get { return pathUpdateInterval; } }
        [SerializeField]
        private float dealDamageInterval = 0.5f;
        public float DealDamageInterval { get { return dealDamageInterval; } }

        private void OnEnable()
        {
            Objective = FindObjectOfType<Objective>();
            Debug.Log(Objective.gameObject.transform.position);
        }
    }
}