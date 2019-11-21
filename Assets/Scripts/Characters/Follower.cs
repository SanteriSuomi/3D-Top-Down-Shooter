using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.AI
{
    public class Follower : Character
    {
        private NavMeshAgent agent;
        private Transform player;
        private WaitForSeconds setDestinationDelay;
        [SerializeField]
        private float offsetFromPlayer = 2.5f;
        [SerializeField]
        private float setDestinationUpdateInterval = 0.25f;
        private bool setDestination;

        protected override void InitializeState()
        {
            agent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<Player.Player>().transform;
            setDestinationDelay = new WaitForSeconds(setDestinationUpdateInterval);
        }

        protected override void StartState()
        {

        }

        protected override void UpdateState()
        {
            if (!setDestination)
            {
                setDestination = true;
                StartCoroutine(SetDestination());
            }
        }

        private IEnumerator SetDestination()
        {
            Vector3 offset = (transform.position - player.position).normalized * offsetFromPlayer;
            agent.SetDestination(player.position + offset);

            yield return setDestinationDelay;
            setDestination = false;
        }

        protected override void OnZeroHP()
        {
            Destroy(gameObject);
        }
    }
}