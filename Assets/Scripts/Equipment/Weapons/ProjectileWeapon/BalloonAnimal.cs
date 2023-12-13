using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BalloonAnimal : ProjectileWeapon {
    [Serializable]
    public class Stats {
        [Tooltip("The sound played when the balloon explodes.")]
        public string explosionSoundName = "";
        [Tooltip("The amount of damage the balloon explosion does.")]
        public float explosionDamage = 100f;
        [Tooltip("The prefab of the explosion.")]
        public GameObject explosionPrefab = null;
        [Tooltip("How long the balloon is on screen before exploding.")]
        public float timeUntilExplosion = 3f;
        [Tooltip("The size of the explosion (in meters).")]
        public float explosionSize = 5f;
        [Tooltip("How much the explosion size is affected by projectile size scaling upgrades.")]
        public float explosionScaling = 0.5f;
        [Tooltip("How quickly the balloon inflates (in percent per second).")]
        public float inflationRate = 1f;
    }

    [SerializeField]
    private Stats BalloonAnimalStats;

    private Stats stats { get => BalloonAnimalStats; }
    public float TimeUntilExplosion { get => stats.timeUntilExplosion; }
    public GameObject ExplosionPrefab { get => stats.explosionPrefab; }
    public string ExplosionSoundName { get => stats.explosionSoundName; }
    public float ExplosionSize { get => stats.explosionSize * 
            1 + statModifiers.size * stats.explosionScaling; }
    public float ExplosionDamage { get => stats.explosionDamage * Damage; }
    public float InflationRate { get => stats.inflationRate; }
}
