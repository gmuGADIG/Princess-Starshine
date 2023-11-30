using System;
using UnityEngine;

[Serializable]
public struct WeaponStats
{
    [Tooltip("The amount of damage each projectile does. Exact details are left to the projectile script.")]
    public float damage;
    
    [Tooltip("Amount of knockback to inflict on enemies hit by the projectiles.")]
    public float knockback;
    
    [Tooltip("Times the weapon fires per second. If set to 0, the weapon will fire at the start and never again.")]
    public float fireRate;
    
    [Tooltip("Projectile scale multiplier. 1 is default size.")]
    public float size;
    
    [Tooltip("How quickly the projectiles move, in meters per second.")]
    public float projectileSpeed;
    
    [Tooltip("Maximum projectiles that can exist at a time. Firing stops when this is reached.")]
    public int maxProjectiles;
    
    [Tooltip("Amount of projectiles to fire with each shot.")]
    public int projectilesPerShot;
    
    [Tooltip("The amount of enemies the projectile can pierce through. 0 means destroy on first hit. -1 means infinite pierce.")]
    public int pierceCount;
}