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
        private NavMeshAgent agent;
        private GameObject target;
        private bool hasSetMovePath;
        private bool isCheckingDistance;

        private enum States
        {
            Idle,
            Move,
            Attack
        }

        private States currentState;

        protected override void InitializeState()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = data.DamageDistance / 2;
        }

        protected override void StartState()
        {
            currentState = States.Move;
        }

        protected override void UpdateState()
        {
            switch (currentState)
            {
                case States.Idle:
                    break;
                case States.Move:
                    CheckRadius(out target);
                    SetPath();
                    break;
                case States.Attack:
                    Attack(target);
                    break;
                default:
                    break;
            }
        }

        private void CheckRadius(out GameObject target)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, data.CheckRadius, data.LayersToDetect);
            target = null;
            if (hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    if (hit.CompareTag("Player"))
                    {
                        target = hit.gameObject;
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
                agent.SetDestination(data.ObjectivePosition);
            }

            yield return new WaitForSeconds(data.PathUpdateInterval);
            hasSetMovePath = false;
        }

        private void Attack(GameObject target)
        {
            if (target != null && agent.enabled)
            {
                agent.SetDestination(target.transform.position);
                if (!isCheckingDistance)
                {
                    isCheckingDistance = true;
                    StartCoroutine(CheckDistance(target));
                }
            }
            else
            {
                currentState = States.Move;
            }
        }

        private IEnumerator CheckDistance(GameObject target)
        {
            CalculateVectorValues(target, out float distance, out float dotProduct);

            if (distance >= data.CheckRadius || dotProduct < data.DotProductMax)
            {
                currentState = States.Move;
            }
            else if (distance <= data.DamageDistance && target.TryGetComponent(out IDamageable player))
            {
            Debug.Log(distance);

                Debug.Log($"{gameObject.name} dealt {data.DamageAmount} damage");
                player.TakeDamage(data.DamageAmount);
            }

            yield return new WaitForSeconds(data.DistanceCheckInterval);
            isCheckingDistance = false;
        }

        private void CalculateVectorValues(GameObject target, out float distance, out float dotProduct)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);
            dotProduct = Vector3.Dot(transform.forward,
            (target.transform.position - transform.position).normalized);
        }

        protected override void OnZeroHP()
        {
            PlayerSettings.GetInstance().Funds += data.FundGiveAmount;
            Destroy(gameObject);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, data.CheckRadius);
        }
        #endif
    }
}