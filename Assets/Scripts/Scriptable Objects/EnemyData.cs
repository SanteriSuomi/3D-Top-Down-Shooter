﻿using UnityEngine;

namespace Shooter.AI
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/Enemy Data", order = 2)]
    public class EnemyData : ScriptableObject
    {
        //
        // Shared data between all enemy AI.
        //
        [SerializeField]
        private LayerMask layersToDetect = default;
        public LayerMask LayersToDetect { get { return layersToDetect; } }
        [SerializeField]
        private float checkRadius = 9;
        public float CheckRadius { get { return checkRadius; } }
        [SerializeField]
        private float damageDistance = 1;
        public float DamageDistance { get { return damageDistance; } }
        [SerializeField]
        private float dotProductMax = 0.25f;
        public float DotProductMax { get { return dotProductMax; } }
        [SerializeField]
        private float fundGiveAmount = 2;
        public float FundGiveAmount { get { return fundGiveAmount; } }
        [SerializeField]
        private float scoreGiveAmount = 1;
        public float ScoreGiveAmount { get { return scoreGiveAmount; } }
        [SerializeField]
        private float damageAmount = 5;
        public float DamageAmount { get { return damageAmount; } }
        [SerializeField]
        private float distanceCheckInterval = 0.25f;
        public float DistanceCheckInterval { get { return distanceCheckInterval; } }
        [SerializeField]
        private float pathUpdateInterval = 0.25f;
        public float PathUpdateInterval { get { return pathUpdateInterval; } }
        [SerializeField]
        private float dealDamageInterval = 0.5f;
        public float DealDamageInterval { get { return dealDamageInterval; } }
        [SerializeField]
        private float rotationSpeed = 60;
        public float RotationSpeed { get { return rotationSpeed; } }
        [SerializeField]
        private float minimumDistanceFromObjective = 1.25f;
        public float MinimumDistanceFromObjective { get { return minimumDistanceFromObjective; } }
        [SerializeField]
        private float objectiveDamageMultiplier = 0.5f;
        public float ObjectiveDamageMultiplier { get { return objectiveDamageMultiplier; } }
        [SerializeField]
        private float healthBarUpdateInterval = 0.1f;
        public float HealthBarUpdateInterval { get { return healthBarUpdateInterval; } }
        [SerializeField]
        private float levitationAmplitude = 0.25f;
        public float LevitationAmplitude { get { return levitationAmplitude; } }
        [SerializeField]
        private float levitationFrequency = 1.5f;
        public float LevitationFrequency { get { return levitationFrequency; } }
    }
}