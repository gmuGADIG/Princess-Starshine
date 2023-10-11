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

    public void Fire()//TODO: Integrate the projectile fired from glitterbomb into the new system, projectile needs to support nested projectiles or glitterbomb's projectile needs to be reworked.
    {

        if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
        {
            Object.Instantiate(projectilePrefab, Player.instance.transform.position, Quaternion.identity);
            timeUntilNextFire = fireRate;
        }
    }

    public override void Update()
    {
        timeUntilNextFire -= Time.deltaTime;
        if (timeUntilNextFire <= 0)
        {
            Fire();
            timeUntilNextFire += 1f / fireRate; // infinity if fireRate is 0
        }
    }

}
