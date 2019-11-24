using Shooter.Utility;
using System.Collections;
using UnityEngine;

namespace Shooter.AI
{
    public class WaveManager : GenericSingleton<WaveManager>
    {
        [SerializeField]
        private Transform[] spawnPoints = default;
        private WaitForSeconds waitForSecondsIntervalDelay;
        [SerializeField]
        private float initialTimerInterval = 5;
        private float timerInterval;
        [SerializeField]
        private int timerDecreaseInterval = 6;
        [SerializeField]
        private float timerDecreaseAmount = 0.1f;
        private float timer;
        private bool decreasedInterval;

        protected override void Awake()
        {
            base.Awake();
            waitForSecondsIntervalDelay = new WaitForSeconds(1);
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
            else if (Mathf.RoundToInt(Time.timeSinceLevelLoad) % timerDecreaseInterval == 0 && !decreasedInterval)
            {
                decreasedInterval = true;
                StartCoroutine(TimerDecreaseDelay());
            }
        }

        private IEnumerator TimerDecreaseDelay()
        {
            timerInterval -= timerDecreaseAmount;
            yield return waitForSecondsIntervalDelay;
            decreasedInterval = false;
        }

        private void SpawnEnemy()
        {
            Enemy.EnemyAI enemy = EnemyPool.GetInstance().Dequeue();
            enemy.gameObject.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position + new Vector3(0, 1, 0);
        }
    }
}