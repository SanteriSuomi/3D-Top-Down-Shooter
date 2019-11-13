using Shooter.AI;
using Shooter.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.Enemy
{
    public class Enemy : Character
    {
        private NavMeshAgent agent;
        private Transform agentObjective;
        private readonly string enemyObjectiveString = "EnemyObjective";

        protected override void InitializeState()
        {
            agent = GetComponent<NavMeshAgent>();
            agentObjective = GameObject.Find(enemyObjectiveString).transform;
        }

        protected override void StartState()
        {
            agent.SetDestination(agentObjective.position);
        }

        protected override void UpdateState()
        {

        }

        protected override void OnZeroHP()
        {
            EnemyPool.GetInstance().Enqueue(this);
        }
    }
}