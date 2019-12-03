using Shooter.Utility;
using System.Collections;
using UnityEngine;

namespace Shooter.AI
{
    public class WaveManager : GenericSingleton<WaveManager>
    {
        [SerializeField]
        private Transform[] spawnPoints = default;
        private WaitForSeconds timerDecreaseDelay;
        [SerializeField]
        private float timerDecreaseDelayInterval = 1;
        [SerializeField]
        private float initialTimerInterval = 5;
        private float timerInterval;
        [SerializeField]
        private float timerDecreaseAmount = 0.1f;
        private float timer;
        [SerializeField]
        private int timerDecreaseInterval = 6;
        private bool decreasedInterval;

        protected override void Awake()
        {
            base.Awake();
            timerDecreaseDelay = new WaitForSeconds(timerDecreaseDelayInterval);
            timerInterval = initialTimerInterval;
        }

        private void Update()
        {
            UpdateWaves();
        }

        private void UpdateWaves()
        {
            timer += Time.deltaTime;
            if (timer >= timerInterval)
            {
                timer = 0;
                SpawnEnemy();
            }
            // Every timerDecreaseInterval..
            else if (Mathf.RoundToInt(Time.timeSinceLevelLoad) % timerDecreaseInterval == 0 && !decreasedInterval)
            {
                decreasedInterval = true;
                // Decrease the timer.
                StartCoroutine(TimerDecreaseDelay());
            }
        }

        private void SpawnEnemy()
        {
            // Get the enemy from the pool.
            Enemy.EnemyGhost enemy = EnemyPool.GetInstance().Dequeue();
            // Spawn enemy at a random spawnpoint in the array.
            enemy.gameObject.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position + new Vector3(0, 1, 0);
        }

        private IEnumerator TimerDecreaseDelay()
        {
            timerInterval -= timerDecreaseAmount;
            yield return timerDecreaseDelay;
            decreasedInterval = false;
        }
    }
}