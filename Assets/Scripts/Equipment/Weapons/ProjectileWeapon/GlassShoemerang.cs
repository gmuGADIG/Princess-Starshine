using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GlassShoemerang : ProjectileWeapon {
    [Tooltip("How long it takes for the projectile to turn around in seconds.")]
    [SerializeField]
    float roundaboutTime = 0.5f;
    
    [Tooltip("How much do speed buffs affect the range of the projectile.")]
    public float DistanceSpeedScaling;

    float speedModifier { get => (1 + statModifiers.projectileSpeed) * (1 + staticStatModifiers.projectileSpeed); }
    public float RoundaboutTime { get => roundaboutTime * (1 + ((speedModifier - 1) * DistanceSpeedScaling )); }

    [Tooltip("How much of the projectile's travel should be deceleration.")]
    public float RoundaboutRatio = .25f;

    [Tooltip("How fast the projectile rotates in degrees per second.")]
    public float RotationalSpeed = 360;
}
