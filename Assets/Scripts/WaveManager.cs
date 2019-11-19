using Shooter.Utility;
using UnityEngine;

namespace Shooter.AI
{
    public class WaveManager : GenericSingleton<WaveManager>
    {
        [SerializeField]
        private Transform[] spawnPoints = default;
        private float timer;
        [SerializeField]
        private float initialTimerInterval = 0.5f;
        [SerializeField]
        private float timerIncreaseInterval = 10;
        [SerializeField]
        private float timerDecreaseAmount = 0.05f; 

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= initialTimerInterval)
            {
                timer = 0;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Enemy.Enemy enemy = EnemyPool.GetInstance().Dequeue();
            enemy.gameObject.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }
    }
}