using Shooter.Shop;
using Shooter.UI;
using Shooter.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.AI
{
    public class Follower : Character, IShopSpawnable
    {
        private NavMeshAgent agent;
        private WaitForSeconds setDestinationDelay;
        private WaitForSeconds dealDamageDelay;
        private Transform player;
        private Transform currentLookAtTarget;
        [SerializeField]
        private LayerMask layersToHit = default;
        [SerializeField]
        private float offsetFromTarget = 2.5f;
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
            // Make sure state has reset on start.
            setDestination = false;
            hasDealtDamage = false;
        }

        protected override void UpdateState()
        {
            // Make sure the AI is enabled before continuing.
            if (enabled && gameObject.activeSelf)
            {
                LookAtPlayer();
                // Get nearby colliders in a sphere radius.
                Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, layersToHit);
                // Store the number of colliders in a radius in a field.
                numberOfHitsInArea = hits.Length;
                if (numberOfHitsInArea > 0)
                {
                    if (!hasDealtDamage && hits[0].CompareTag("Enemy"))
                    {
                        hasDealtDamage = true;
                        GetEnemy(hits);
                    }
                    else if (!setDestination)
                    {
                        setDestination = true;
                        UpdateRotationTargetToPlayer(hits[0].transform);
                        StartCoroutine(SetDestination(hits[0].transform.position));
                    }
                }
            }
        }

        private void LookAtPlayer()
        {
            // Constantly rotate toward the target.
            transform.LookAt(currentLookAtTarget);
        }

        private void UpdateRotationTargetToPlayer(Transform target)
        {
            // Make sure there are enough hits in the area...
            if (numberOfHitsInArea <= 0)
            {
                // And then update the look target.
                currentLookAtTarget = target;
            }
        }

        private IEnumerator SetDestination(Vector3 targetPos)
        {
            // Calculate a position offset from the target to follow.
            Vector3 offset = (transform.position - targetPos).normalized * offsetFromTarget;
            // Update the destination to be the target position + the offset.
            agent.SetDestination(targetPos + offset);
            yield return setDestinationDelay;
            setDestination = false;
        }

        private void GetEnemy(Collider[] hits)
        {
            // Attempt to get the enemy component for damaging the enemy.
            if (hits[0].TryGetComponent(out IDamageable enemy))
            {
                StartCoroutine(DealDamage(enemy));
            }
            else
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Enemy is null.");
                #endif
            }
        }

        private IEnumerator DealDamage(IDamageable enemy)
        {
            UpdateRotationTargetToEnemy(enemy);
            // Damage the target.
            enemy.TakeDamage(damage);
            yield return dealDamageDelay;
            hasDealtDamage = false;
        }

        private void UpdateRotationTargetToEnemy(IDamageable enemy)
        {
            // Cast the enemy interface to a monobehaviour so we can get the target transform.
            MonoBehaviour enemyObject = enemy as MonoBehaviour;
            currentLookAtTarget = enemyObject.transform;
        }

        protected override void OnZeroHP()
        {
            // If the follower dies, make sure to update the amount of followers counter.
            ShopSpawner.GetInstance().AmountOfFollowers -= 1;
            FollowerPool.GetInstance().Enqueue(this);
        }

        public void SpawnItem(float spawnRange)
        {
            Follower spawnedObject = FollowerPool.GetInstance().Dequeue();
            Vector3 spawnPos = player.position + new Vector3(Random.Range(-spawnRange, spawnRange),
                0, Random.Range(-spawnRange, spawnRange));
            spawnedObject.transform.position = spawnPos;
        }
    }
}