using System;
using UnityEngine;
public class Laser : MonoBehaviour
{
    /** Amount of knockback to inflict on enemies hit by the projectiles. */
    [SerializeField] protected float knockback;
    /** The amount of damage each projectile does. Exact details are left to the projectile script. */
    [SerializeField] protected float damage;
    /** The amount of enemies the projectile can pierce through. 0 means destroy on first hit. -1 means infinite pierce. */
    [SerializeField] protected int pierceCount;
    /** The prefab for the laser */
    [SerializeField] protected Material laserMaterial;
    bool hasBeenSetUp = true;

    protected new void Update()
    {
        if (!hasBeenSetUp) throw new Exception("Projectile has not been set up!");
    }

    public void Setup(float damage, int pierceCount, float knockback)
    {
        this.pierceCount = pierceCount;
        this.damage = damage;
        this.knockback = knockback;
    }
}
