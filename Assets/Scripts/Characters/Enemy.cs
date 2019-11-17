using Shooter.AI;
using Shooter.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.Enemy
{
    public class Enemy : Character
    {
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
            Collider[] hits = Physics.OverlapSphere(transform.position, EnemyShared.CheckRadius, EnemyShared.LayersToDetect);
            target = null;
            if (hits.Length > 0)
            {
                target = hits[0].gameObject;
                hasSetMovePath = false;
                currentState = States.Attack;
            }
        }

        private void SetPath()
        {
            if (!hasSetMovePath)
            {
                hasSetMovePath = true;
                agent.SetDestination(EnemyShared.ObjectivePos);
            }
        }

        private void Attack(GameObject target)
        {
            if (target != null)
            {
                Vector3 targetPos = target.transform.position;
                agent.SetDestination(targetPos);
                if (!isCheckingDistance)
                {
                    isCheckingDistance = true;
                    StartCoroutine(CheckDistance(targetPos));
                }
            }
            else
            {
                currentState = States.Move;
            }
        }

        private IEnumerator CheckDistance(Vector3 targetPos)
        {
            if (Vector3.Distance(targetPos, transform.position) >= EnemyShared.CheckRadius + EnemyShared.CheckRadius / 2)
            {
                currentState = States.Move;
            }

            yield return new WaitForSeconds(EnemyShared.DistanceCheckInterval);

            isCheckingDistance = false;
        }

        protected override void OnZeroHP()
        {
            PlayerSettings.GetInstance().Funds += EnemyShared.FundGiveAmount;
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, EnemyShared.CheckRadius);
        }
#endif
    }
}