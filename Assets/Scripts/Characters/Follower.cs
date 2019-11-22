using Shooter.UI;
using Shooter.Utility;
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
        private WaitForSeconds dealDamageDelay;
        private Transform currentLookAtTarget;
        [SerializeField]
        private LayerMask layersToHit = default;
        [SerializeField]
        private float offsetFromPlayer = 2.5f;
        [SerializeField]
        private float setDestinationUpdateInterval = 0.25f;
        [SerializeField]
        private float dealDamageDelayInterval = 0.25f;
        [SerializeField]
        private float damage = 5;
        [SerializeField]
        private float damageRadius = 7;
        private int numberOfHitsInArea;
        private bool setDestination;
        private bool hasDealtDamage;

        protected override void InitializeState()
        {
            agent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<Player.Player>().transform;
            setDestinationDelay = new WaitForSeconds(setDestinationUpdateInterval);
            dealDamageDelay = new WaitForSeconds(dealDamageDelayInterval);
        }

        protected override void StartState()
        {
            // Intentionally left empty.
        }

        protected override void UpdateState()
        {
            transform.LookAt(currentLookAtTarget);
            Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, layersToHit);
            numberOfHitsInArea = hits.Length;
            if (numberOfHitsInArea > 0 && hits[0].CompareTag("Enemy") && !hasDealtDamage)
            {
                hasDealtDamage = true;
                hits[0].TryGetComponent(out IDamageable enemy);
                if (enemy != null)
                {
                    StartCoroutine(DealDamage(enemy));
                }
                else
                {
                    #if UNITY_EDITOR
                    Debug.Log("Enemy is null");
                    #endif
                }
            }
            else if (!setDestination)
            {
                setDestination = true;
                StartCoroutine(SetDestination());
            }
        }

        private IEnumerator DealDamage(IDamageable enemy)
        {
            UpdateRotationToEnemy(enemy);
            enemy.TakeDamage(damage);
            yield return dealDamageDelay;
            hasDealtDamage = false;
        }

        private void UpdateRotationToEnemy(IDamageable enemy)
        {
            MonoBehaviour enemyObject = enemy as MonoBehaviour;
            currentLookAtTarget = enemyObject.transform;
        }

        private IEnumerator SetDestination()
        {
            UpdateRotationToPlayer();
            Vector3 offset = (transform.position - player.position).normalized * offsetFromPlayer;
            agent.SetDestination(player.position + offset);
            yield return setDestinationDelay;
            setDestination = false;
        }

        private void UpdateRotationToPlayer()
        {
            if (numberOfHitsInArea <= 0)
            {
                currentLookAtTarget = player;
            }
        }

        protected override void OnZeroHP()
        {
            ShopSpawner.GetInstance().AmountOfFollowers -= 1;
            Destroy(gameObject);
        }
    }
}