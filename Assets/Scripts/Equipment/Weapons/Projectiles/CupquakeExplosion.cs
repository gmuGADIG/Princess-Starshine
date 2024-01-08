using System;
using UnityEngine;

class CupquakeExplosion : Projectile {
    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate)
    {
        // use CupquakeExplosion.SetupExt
        throw new NotSupportedException();
    }

    public void SetupExt(ProjectileWeapon weapon, Vector2 target, float damage, 
        int pierceCount, float speed, float knockback, float size, float dotRate,
        bool alt)
    {
        base.Setup(weapon, target, damage, pierceCount, speed, knockback, size, dotRate);
    
        var particleSystem = GetComponentInChildren<ParticleSystem>().main;
        particleSystem.startColor = alt ? new Color(0.565f, 0.49f, 0.976f) : new Color(0.976f, 0.49f, 0.576f);
    }
}