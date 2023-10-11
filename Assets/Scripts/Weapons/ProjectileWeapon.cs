using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    //The number of projectiles fired from the weapon on "use"
    [SerializeField]
    protected int numProjectiles;

    //The vector2 direction to shoot at, Normalized
    //[SerializeField]
    //protected Vector2 fireDirectionNormalized;

    //The uniform spread between projectiles
    //[SerializeField]
    //protected float fireSpreadAngleDegrees;

    //Variance added onto the uniform spread
    //[SerializeField]
    //protected float spreadVarianceDegrees;

    //The speed of the projectiles fired from this weapon
    //[SerializeField]
    //protected float projectileSpeed;

    //The number of enemies the projectile needs to hit before dissapearing. Set to 0 to ignore enemies.
    //[SerializeField]
    //protected float projectilePierce;
    //
    //  ^
    //  I
    //  I
    //move these fields to the projectile itself if you need them

    [SerializeField]
    protected static GameObject projectilePrefab;

    public abstract void Fire();

    public abstract bool CanFire();

}
