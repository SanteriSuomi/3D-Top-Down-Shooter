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
        private bool setInitialTarget;
        private IDamageable playerTarget;
        private WaitForSeconds objectiveDamageDelay;
        private WaitForSeconds checkDistanceTo;

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
        }

        protected override void StartState()
        {
            ResetBools();
            currentState = States.Move;
        }

        private void ResetBools()
        {
            hasSetMovePath = false;
            isCheckingDistance = false;
            hasDealtDamageToObjective = false;
            setInitialTarget = false;
        }

        protected override void UpdateState()
        {
            InitialTarget();
            RotateDirectionToTarget(currentTarget);
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
                    Attack(currentTarget);
                    break;
                case States.Objective:
                    DealDamageToObjective();
                    break;
                default:
                    break;
            }
        }

        private void InitialTarget()
        {
            if (!setInitialTarget)
            {
                setInitialTarget = true;
                currentTarget = objective.gameObject;
            }
        }

        private void RotateDirectionToTarget(GameObject target)
        {
            if (target != null)
            {
                Quaternion lookDirection = Quaternion.LookRotation((target.transform.position - transform.position).normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, data.RotationSpeed * Time.deltaTime);
            }
        }

        private void CheckRadius()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, data.CheckRadius, data.LayersToDetect);
            currentTarget = null;
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

        private void CheckDistanceFromObjective()
        {
            if (Vector3.Distance(transform.position, data.ObjectivePosition) <= data.MinimumDistanceFromObjective)
            {
                currentState = States.Objective;
            }
        }

        private IEnumerator PathDelay()
        {
            if (agent.enabled)
            {
                agent.SetDestination(data.ObjectivePosition);
            }

            yield return new WaitForSeconds(data.PathUpdateInterval);
            hasSetMovePath = false;
        }

        private void Attack(GameObject target)
        {
            if (target != null && agent.enabled)
            {
                RotateDirectionToTarget(target);
                if (!isCheckingDistance)
                {
                    isCheckingDistance = true;
                    agent.SetDestination(target.transform.position);
                    StartCoroutine(CheckDistanceTo(target));
                }
            }
            else
            {
                currentState = States.Move;
            }
        }

        private IEnumerator CheckDistanceTo(GameObject target)
        {
            dealDamageTimer += Time.deltaTime;
            CalculateVectorValues(target, out float distance, out float dotProduct);
            ActWithDistance(target, distance, dotProduct);

            yield return checkDistanceTo;
            isCheckingDistance = false;
        }

        private void ActWithDistance(GameObject target, float distance, float dotProduct)
        {
            RetrieveDamageable(target);

            if (distance >= data.CheckRadius || dotProduct < data.DotProductMax)
            {
                currentState = States.Move;
            }
            else if (distance <= data.DamageDistance && dealDamageTimer >= data.DealDamageInterval)
            {
                playerTarget.TakeDamage(data.DamageAmount);
                #if UNITY_EDITOR
                Debug.Log($"Dealt {data.DamageAmount} damage to {playerTarget}. It now has {playerTarget.Hitpoints} left.");
                #endif
            }
        }

        private void RetrieveDamageable(GameObject target)
        {
            if (playerTarget == null)
            {
                playerTarget = target.GetComponent<IDamageable>();
            }
        }

        private void CalculateVectorValues(GameObject target, out float distance, out float dotProduct)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);
            dotProduct = Vector3.Dot(transform.forward,
            (target.transform.position - transform.position).normalized);
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
            Debug.Log($"Dealt damage to objective. {objective.Hitpoints}");
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