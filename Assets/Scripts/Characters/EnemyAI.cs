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
        // Scriptable object data container that's shared along all enemy instances.
        [SerializeField]
        private EnemyData data = default;

        private Transform mainCamera;
        private Slider healthBar;
        private Objective objective;
        private NavMeshAgent agent;
        private GameObject currentTarget;
        private AudioSource audioSource;

        private WaitForSeconds objectiveDamageDelay;
        private WaitForSeconds checkDistanceTo;
        private WaitForSeconds setPathDelay;

        private IDamageable damageTarget;

        private float dealDamageTimer;
        private float healthBarUpdateTimer;

        private bool hasSetMovePath;
        private bool isCheckingDistance;
        private bool hasDealtDamageToObjective;
        private bool isTargetingObjective;

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
            // Create instances and apply references on awake.
            mainCamera = Camera.main.transform;
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
            // Make sure to reset AI state every time it gets de-pooled.
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
            isTargetingObjective = false;
        }

        protected override void UpdateState()
        {
            // Make sure the component is and gameObject is enabled, to prevent errors when getting re-pooled.
            if (enabled && gameObject.activeSelf)
            {
                // Methods not in the state machine get updated regardless of what state the AI is in.
                Levitate();
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
                        DoDamageToObjective();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Levitate()
        {
            // Apply a sine wave to the Y transform of the object to make a levitation effect.
            transform.position = new Vector3(transform.position.x,
                transform.position.y + Mathf.Sin(Time.time * data.LevitationFrequency) * data.LevitationAmplitude,
                transform.position.z);
        }

        private void UpdateHealthBar()
        {
            // Update the healthbar according to a timer, as it doesn't need to get updated each frame.
            healthBarUpdateTimer += Time.deltaTime;
            if (healthBarUpdateTimer >= data.HealthBarUpdateInterval)
            {
                healthBarUpdateTimer = 0;
                healthBar.value = HitPoints;
                // Make healthbar look at the player camera.
                healthBar.transform.LookAt(mainCamera);
            }
        }

        private void RotateDirectionToTarget()
        {
            if (currentTarget != null)
            {
                // Create a look rotation to the current target and apply a smooth rotation.
                Quaternion lookDirection = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, data.RotationSpeed * Time.deltaTime);
            }
        }

        private void CheckRadius()
        {
            // Check for enemy collisions in a sphere around the AI.
            Collider[] hits = Physics.OverlapSphere(transform.position, data.CheckRadius, data.LayersToDetect);
            if (hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    // Register the hit if it has an IDamageable interface, or, to be sure, a player component.
                    if (hit.TryGetComponent(out IDamageable _) || hit.TryGetComponent(out Player.Player _))
                    {
                        // Switch the target to be the hit gameObject (for rotation etc).
                        currentTarget = hit.gameObject;
                        currentState = States.Attack;
                    }
                }
            }
        }

        private void SetPath()
        {
            // Agent doesn't need an updated path each frame.
            if (!hasSetMovePath)
            {
                hasSetMovePath = true;
                StartCoroutine(SetPathDelay());
            }
        }

        private IEnumerator SetPathDelay()
        {
            agent.SetDestination(objective.transform.position);
            yield return setPathDelay;
            hasSetMovePath = false;
        }

        private void CheckDistanceFromObjective()
        {
            // If AI is near the final objective, force the state to objective.
            if (Vector3.Distance(transform.position, objective.transform.position) <= data.MinimumDistanceFromObjective)
            {
                currentState = States.Objective;
            }
        }

        private void Attack()
        {
            // Update the damage timer whilst on attack state.
            dealDamageTimer += Time.deltaTime;
            if (currentTarget != null && !isCheckingDistance)
            {
                isCheckingDistance = true;
                // Update the destination to the target.
                agent.SetDestination(currentTarget.transform.position);
                StartCoroutine(AttackAction());
            }
            else
            {
                currentState = States.Move;
            }
        }

        private IEnumerator AttackAction()
        {
            GetDistanceAndDotProduct(out float distance, out float dotProduct);
            CheckDistanceAndDotProduct(distance, dotProduct);
            yield return checkDistanceTo;
            isCheckingDistance = false;
        }

        private void GetDistanceAndDotProduct(out float distance, out float dotProduct)
        {
            // Calculate the distance and the dot product (is/is not behind) of this gameObject relative to the target.
            distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            dotProduct = Vector3.Dot(transform.forward,
            (currentTarget.transform.position - transform.position).normalized);
        }

        private void CheckDistanceAndDotProduct(float distance, float dotProduct)
        {
            // If the distance or dot product is over & under (respectively) a certain threshold, switch the state to move.
            if (distance >= data.CheckRadius || dotProduct < data.DotProductMax)
            {
                currentState = States.Move;
            }
            // Else if it's over that threshold, attack the target.
            else if (distance <= data.DamageDistance && dealDamageTimer >= data.DealDamageInterval)
            {
                dealDamageTimer = 0;
                PlayAttackSound();
                AttemptToDamageTarget();
            }
        }

        private void AttemptToDamageTarget()
        {
            // Check if it's the player, and damage the player..
            if (currentTarget.TryGetComponent(out PlayerSettings playerSettings))
            {
                playerSettings.HitPoints -= data.DamageAmount;
            }
            // Else try to get a damageable component and damge using that.
            else if (currentTarget.TryGetComponent(out damageTarget))
            {
                damageTarget.TakeDamage(data.DamageAmount);
            }
        }

        private void DoDamageToObjective()
        {
            if (!hasDealtDamageToObjective)
            {
                if (!isTargetingObjective)
                {
                    // Update rotation target to the objective only once per instance.
                    isTargetingObjective = true;
                    currentTarget = objective.gameObject;
                }
                hasDealtDamageToObjective = true;
                StartCoroutine(ObjectiveDamageDelay());
            }
        }

        private IEnumerator ObjectiveDamageDelay()
        {
            PlayAttackSound();
            // Deal damage to objective using it's interface.
            objective.TakeDamage(data.DamageAmount);
            yield return objectiveDamageDelay;
            hasDealtDamageToObjective = false;
        }

        private void PlayAttackSound()
        {
            // Play an attack sound unless it's already playing.
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        protected override void OnZeroHP()
        {
            // When AI zero get's to be 0 or under, reward the player and re-pool this object.
            PlayerSettings.GetInstance().Funds += data.FundGiveAmount;
            PlayerSettings.GetInstance().Score += data.ScoreGiveAmount;
            EnemyPool.GetInstance().Enqueue(this);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Helper gizmo for visualizing the checksphere of the AI.
            Gizmos.DrawWireSphere(transform.position, data.CheckRadius);
        }
        #endif
    }
}