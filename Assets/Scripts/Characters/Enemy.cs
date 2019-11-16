using Shooter.AI;
using Shooter.Player;
using Shooter.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.Enemy
{
    public class Enemy : Character
    {
        [SerializeField]
        private float fundGiveAmount = 2;
        private NavMeshAgent agent;

        protected override void InitializeState()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        protected override void StartState()
        {
            agent.SetDestination(EnemySharedData.Objective.position);
        }

        protected override void UpdateState()
        {

        }

        protected override void OnZeroHP()
        {
            PlayerSettings.GetInstance().Funds += fundGiveAmount;
            EnemyPool.GetInstance().Enqueue(this);
        }
    }
}