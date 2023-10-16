using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyWand : ProjectileWeapon
{
    public FairyWand()
    {
        fireRate = 0.5f;

        knockback = 2f;

        projectileSize = 0.5f;

        damage = 25f;

        projectileCount = 1;

        projectileSpeed = 2f;

        pierceCount = 0;

        targetingStrategy = TargetType.NearestEnemy;

        spawnProjectileAtTarget = false;

        projectileLocalSpace = true;

        projectilePrefab = Resources.Load<GameObject>("Projectiles/FairyWandProjectile");
    }

}
