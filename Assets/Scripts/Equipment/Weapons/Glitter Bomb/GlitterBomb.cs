using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterBomb : ProjectileWeapon
{

    public GlitterBomb()
    {
        /** Times the weapon fires per second. If set to 0, the weapon will fire at the start and never again. */
        fireRate = 1.0f;

        /** Amount of knockback to inflict on enemies hit by the projectiles. */
        knockback = 0f;

        /** Projectile size multiplier. Used for level-ups. */
        projectileSize = 1f;

        /** The amount of damage each projectile does. Exact details are left to the projectile script. */
        damage = 0f;

        /** Amount of projectiles to fire with each shot. */
        projectileCount = 1;

        /** How fast the projectiles start out when fired. */
        projectileSpeed = 1.0f;

        /** The amount of enemies the projectile can pierce through. 0 means destroy on first hit. -1 means infinite pierce. */
        pierceCount = -1;

        /** How the weapon should determine which direction to fire in */
        targetingStrategy = TargetType.RandomEnemy;

        /** True if the projectile should spawn at its target, as opposed to emitting from the player. */
        spawnProjectileAtTarget = false;

        /** True if the projectile should be attached to the player and move with them. Otherwise, it operates in world space and moves independently of the player. */
        projectileLocalSpace = false;

        projectilePrefab = Resources.Load<GameObject>("Projectiles/GlitterBomb/GlitterPrimary");
    }
}
