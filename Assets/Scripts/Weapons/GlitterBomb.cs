using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterBomb : ProjectileWeapon
{
    
    public GlitterBomb()
    {
        numProjectiles = 1;
        cooldownTimeSeconds = 1.0F;
        projectilePrefab = Resources.Load<GameObject>("Projectiles/GlitterBomb/GlitterPrimary");
    }

    public override void Fire()
    {
        Object.Instantiate(projectilePrefab, Player.Instance.transform.position, Quaternion.identity);
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
