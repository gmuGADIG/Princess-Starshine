using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileCollision : MonoBehaviour
{
    float damage = 0;
    bool isDamageSet = false;

    Collider2D collider;
    ContactFilter2D filter;
    Collider2D[] collisions = new Collider2D[16];

    void Start()
    {
        collider = GetComponent<Collider2D>();
        filter = new ContactFilter2D();
        filter.NoFilter();
    }

    void Update()
    {
        if (!isDamageSet)
        {
            Destroy(this.gameObject);
            throw new Exception("Projectile Collision's damage was not set! Perhaps the projectile's Start method was overridden?");
        }
        
        int hitCount = collider.OverlapCollider(filter, collisions);
        // print("hitCount = " + hitCount);
        foreach (var col in collisions.Take(hitCount))
        {
            if (col.CompareTag("Enemy"))
            {
                var enemy = col.GetComponent<EnemyTemplate>();
                if (enemy == null) throw new Exception("Object with tag `Enemy` did not have an `EnemyTemplate` script!");
                
                enemy.TakeDamage(this.damage);
            }
            else if (col.CompareTag("Boss"))
            {
                BossHealth bossHealth = col.GetComponent<BossHealth>();
                bossHealth.Damage(damage);
            }
        }
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
        this.isDamageSet = true;
    }
}
