using UnityEngine;
using UnityEngine.AI;

namespace Shooter.AI
{
    public class Follower : Character
    {
        private NavMeshAgent agent;
        private Transform player;

        protected override void InitializeState()
        {
            agent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<Player.Player>().transform;
        }

        protected override void StartState()
        {

        }

        protected override void UpdateState()
        {
            Vector3 offPlacement = (transform.position - player.position).normalized * 2.5f;
            Debug.Log(offPlacement);
            if (Vector3.Distance(transform.position, player.position) <= 5)
            {
                agent.SetDestination(player.position + offPlacement);
            }
        }

        protected override void OnZeroHP()
        {
            Destroy(gameObject);
        }
    }
}