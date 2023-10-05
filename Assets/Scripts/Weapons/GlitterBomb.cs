using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterBomb : ProjectileWeapon
{
    
    public GlitterBomb()
    {
        numProjectiles = 1;
        fireDirectionAngleDegrees = 0;
        fireSpreadAngleDegrees = 0;
        spreadVarianceDegrees = 0;
        projectileSpeed = 1.0F;
        projectilePierce = 1;
        cooldownTimeSeconds = 1.0F;
        projectilePrefab = Resources.Load<GameObject>("Projectiles/GlitterBomb/GlitterPrimary");
    }

    public override void Fire()
    {   
        var newProj = Object.Instantiate(projectilePrefab, Player.Instance.transform.position, Quaternion.identity);
        newProj.GetComponent<GlitterPrimary>().GetFired(Random.Range(0.0f, 360.0f), projectileSpeed);

        currentCooldownTime = cooldownTimeSeconds;
    }

    public override bool CanFire()
    {
        return currentCooldownTime == 0F;
    }

    public override void Update()
    {
        currentCooldownTime = Mathf.Max(0F,currentCooldownTime-Time.deltaTime);
        if (CanFire())
        {
            Fire();

        }

    }

}
