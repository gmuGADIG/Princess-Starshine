using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalrogCoinProjectile : Projectile
{

    //Currently nothing will add as things are needed
    public virtual void Setup(Vector2 target, float damage, int pierceCount, float speed, float knockback, float size)
    {

        velocity = target * speed;
        this.pierceCount = pierceCount;
        this.damage = damage;
        this.knockback = knockback;
        transform.localScale = new Vector2(size, size);
        hasBeenSetUp = true;
        maxLifeTime = 10;
    }

    protected override void OnDestroy()
    {
        //Shouldn't do nothing
    }

}
