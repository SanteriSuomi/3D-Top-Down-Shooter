using Shooter.AI;
using Shooter.Player;
using Shooter.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.Enemy
{
    public class Enemy : Character
    {
        [SerializeField]
        private EnemyData data = default;
        private Objective objective;
        private NavMeshAgent agent;
        private GameObject currentTarget;
        private float dealDamageTimer;
        private bool hasSetMovePath;
        private bool isCheckingDistance;
        private bool hasDealtDamageToObjective;
        private bool setInitialRotation;
        private PlayerSettings playerTarget;
        private WaitForSeconds objectiveDamageDelay;
        private WaitForSeconds checkDistanceTo;
        private WaitForSeconds setPathDelay;

        private enum States
        {
            Idle,
            Move,
            Attack,
            Objective
        }

        private States currentState;

        protected override void InitializeState()
        {
            agent = GetComponent<NavMeshAgent>();
            objective = FindObjectOfType<Objective>();
            objectiveDamageDelay = new WaitForSeconds(data.DealDamageInterval);
            checkDistanceTo = new WaitForSeconds(data.DistanceCheckInterval);
            setPathDelay = new WaitForSeconds(data.PathUpdateInterval);
        }

        protected override void StartState()
        {
            ResetBools();
            currentTarget = objective.gameObject;
            currentState = States.Move;
        }

        private void ResetBools()
        {
            hasSetMovePath = false;
            isCheckingDistance = false;
            hasDealtDamageToObjective = false;
            setInitialRotation = false;
        }

        protected override void UpdateState()
        {
            RotateDirectionToTarget();
            switch (currentState)
            {
                case States.Idle:
                    break;
                case States.Move:
                    CheckRadius();
                    SetPath();
                    CheckDistanceFromObjective();
                    break;
                case States.Attack:
                    Attack();
                    break;
                case States.Objective:
                    DealDamageToObjective();
                    break;
                default:
                    break;
            }
        }
        
        private void RotateDirectionToTarget()
        {
            if (currentTarget != null)
            {
                Quaternion lookDirection = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized, Vector3.up);

                if (!setInitialRotation)
                {
                    setInitialRotation = true;
                    transform.rotation = Quaternion.Euler(lookDirection.eulerAngles);
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, data.RotationSpeed * Time.deltaTime);
            }
        }

        private void CheckRadius()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, data.CheckRadius, data.LayersToDetect);
            if (hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    if (hit.CompareTag("Player"))
                    {
                        currentTarget = hit.gameObject;
                    }
                }

                currentState = States.Attack;
            }
        }

        private void SetPath()
        {
            if (!hasSetMovePath)
            {
                hasSetMovePath = true;
                StartCoroutine(PathDelay());
            }
        }

        private IEnumerator PathDelay()
        {
            if (agent.enabled)
            {
                agent.SetDestination(objective.transform.position);
            }

            yield return setPathDelay;
            hasSetMovePath = false;
        }

        private void CheckDistanceFromObjective()
        {
            if (Vector3.Distance(transform.position, objective.transform.position) <= data.MinimumDistanceFromObjective)
            {
                currentState = States.Objective;
            }
        }

        private void Attack()
        {
            if (currentTarget != null && agent.enabled)
            {
                if (!isCheckingDistance)
                {
                    isCheckingDistance = true;
                    agent.SetDestination(currentTarget.transform.position);
                    StartCoroutine(CheckDistanceTo());
                }
            }
            else
            {
                currentState = States.Move;
            }
        }

        private IEnumerator CheckDistanceTo()
        {
            CalculateVectorValues(out float distance, out float dotProduct);
            ActWithDistance(distance, dotProduct);

            yield return checkDistanceTo;
            isCheckingDistance = false;
        }

        private void CalculateVectorValues(out float distance, out float dotProduct)
        {
            distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            dotProduct = Vector3.Dot(transform.forward,
            (currentTarget.transform.position - transform.position).normalized);
        }

        private void ActWithDistance(float distance, float dotProduct)
        {
            dealDamageTimer += Time.deltaTime;
            RetrieveDamageable();

            if (distance >= data.CheckRadius || dotProduct < data.DotProductMax)
            {
                currentState = States.Move;
            }
            else if (distance <= data.DamageDistance && dealDamageTimer >= data.DealDamageInterval)
            {
                //playerTarget.TakeDamage(data.DamageAmount);
                playerTarget.HitPoints -= data.DamageAmount;
                #if UNITY_EDITOR
                Debug.Log($"Dealt {data.DamageAmount} damage to {playerTarget}. It now has {playerTarget.HitPoints} left.");
                #endif
            }
        }

        private void RetrieveDamageable()
        {
            if (playerTarget == null)
            {
                playerTarget = currentTarget.GetComponent<PlayerSettings>();
            }
        }

        private void DealDamageToObjective()
        {
            if (!hasDealtDamageToObjective)
            {
                hasDealtDamageToObjective = true;
                StartCoroutine(ObjectiveDamageDelay());
            }
        }

        private IEnumerator ObjectiveDamageDelay()
        {
            objective.TakeDamage(data.DamageAmount);
            #if UNITY_EDITOR
            Debug.Log($"Dealt damage to objective. {objective.HitPoints}");
            #endif
            yield return objectiveDamageDelay;
            hasDealtDamageToObjective = false;
        }

        protected override void OnZeroHP()
        {
            PlayerSettings.GetInstance().Funds += data.FundGiveAmount;
            EnemyPool.GetInstance().Enqueue(this);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, data.CheckRadius);
        }
        #endif
    }
}