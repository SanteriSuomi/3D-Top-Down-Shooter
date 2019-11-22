using Shooter.AI;
using Shooter.Player;
using Shooter.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Shooter.Enemy
{
    public class EnemyAI : Character
    {
        [SerializeField]
        private EnemyData data = default;
        private new Transform camera;
        private Slider healthBar;
        private Objective objective;
        private NavMeshAgent agent;
        private GameObject currentTarget;
        private AudioSource audioSource;
        private float dealDamageTimer;
        private bool hasSetMovePath;
        private bool isCheckingDistance;
        private bool hasDealtDamageToObjective;
        private IDamageable damageTarget;
        private WaitForSeconds objectiveDamageDelay;
        private WaitForSeconds checkDistanceTo;
        private WaitForSeconds setPathDelay;
        private float healthBarUpdateTimer;

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
            camera = Camera.main.transform;
            agent = GetComponent<NavMeshAgent>();
            healthBar = GetComponentInChildren<Slider>();
            healthBar.maxValue = startingHitPoints;
            objective = FindObjectOfType<Objective>();
            audioSource = GetComponent<AudioSource>();
            objectiveDamageDelay = new WaitForSeconds(data.DealDamageInterval);
            checkDistanceTo = new WaitForSeconds(data.DistanceCheckInterval);
            setPathDelay = new WaitForSeconds(data.PathUpdateInterval);
        }

        protected override void StartState()
        {
            ResetState();
            currentTarget = objective.gameObject;
            currentState = States.Move;
        }

        private void ResetState()
        {
            HitPoints = startingHitPoints;
            healthBar.value = startingHitPoints;
            hasSetMovePath = false;
            isCheckingDistance = false;
            hasDealtDamageToObjective = false;
        }

        protected override void UpdateState()
        {
            UpdateHealthBar();
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

        private void UpdateHealthBar()
        {
            healthBarUpdateTimer += Time.deltaTime;
            if (healthBarUpdateTimer >= data.HealthBarUpdateInterval)
            {
                healthBarUpdateTimer = 0;
                healthBar.value = HitPoints;
                healthBar.transform.LookAt(camera);
            }
        }

        private void RotateDirectionToTarget()
        {
            if (currentTarget != null)
            {
                Quaternion lookDirection = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, data.RotationSpeed * Time.deltaTime);
            }
        }

        private void CheckRadius()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, data.CheckRadius, data.LayersToDetect);
            if (hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    if (hit.TryGetComponent(out IDamageable _) || hit.TryGetComponent(out Player.Player _))
                    {
                        currentTarget = hit.gameObject;
                    }
                }

                currentState = States.Attack;
            }
        }

        private void SetPath()
        {
            if (!hasSetMovePath && gameObject.activeSelf)
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
            dealDamageTimer += Time.deltaTime;
            if (currentTarget != null && !isCheckingDistance && enabled)
            {
                isCheckingDistance = true;
                agent.SetDestination(currentTarget.transform.position + new Vector3(0, 0.5f, 0));
                StartCoroutine(CheckDistanceTo());
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
            RetrieveDamageComponent();

            if (distance >= data.CheckRadius || dotProduct < data.DotProductMax)
            {
                currentState = States.Move;
            }
            else if (distance <= data.DamageDistance && dealDamageTimer >= data.DealDamageInterval)
            {
                dealDamageTimer = 0;
                PlayAttackSound();
                if (currentTarget.TryGetComponent(out PlayerSettings playerSettings))
                {
                    playerSettings.HitPoints -= data.DamageAmount;
                }
                else
                {
                    damageTarget.TakeDamage(data.DamageAmount);
                }
                #if UNITY_EDITOR
                Debug.Log($"Dealt {data.DamageAmount} damage to {damageTarget}. It now has {damageTarget.HitPoints} left.");
                #endif
            }
        }

        private void RetrieveDamageComponent()
        {
            damageTarget = currentTarget.GetComponent<IDamageable>();
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
            PlayAttackSound();
            objective.TakeDamage(data.DamageAmount);
            #if UNITY_EDITOR
            Debug.Log($"Dealt damage to objective. {objective.HitPoints}");
            #endif
            yield return objectiveDamageDelay;
            hasDealtDamageToObjective = false;
        }

        private void PlayAttackSound()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
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