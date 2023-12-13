using UnityEngine;

class LovelyLullabyProjectile : Projectile {
    public override void OnWeaponLevelUp(float newDamage, int newPierceCount, float newSpeed, float newKnockback, float newSize, float newDotRate) { 
        this.damage = newDamage;
        this.pierceCount = newPierceCount;
        this.speed = newSpeed;
        this.knockback = newKnockback;

        transform.localScale = new Vector3(1,1,1) * newSize;
        
        this.dotRate = newDotRate;

        projectileCollision.damage = newDamage;
        projectileCollision.knockback = newKnockback;
        projectileCollision.hitsPerSecond = dotRate;
    }
}
