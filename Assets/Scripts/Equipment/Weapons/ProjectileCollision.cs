using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileCollision : MonoBehaviour
{
    float damage = 0;
    float hitsPerSecond = 1;
    bool isSetUp = false;

    Collider2D collider;
    ContactFilter2D filter;
    Collider2D[] collisions = new Collider2D[16];
    Dictionary<Collider2D, float> pastCollisionTimes = new();

    public event Action onHit;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        filter = new ContactFilter2D();
        filter.NoFilter();
    }

    void Update()
    {
        if (!isSetUp)
        {
            Destroy(this.gameObject);
            throw new Exception("ProjectileCollision was not set up! Maybe the projectile's Start method was overridden?");
        }
        
        int hitCount = collider.OverlapCollider(filter, collisions);
        // print("hitCount = " + hitCount);
        foreach (var col in collisions.Take(hitCount))
        {
            // if this collision was hit recently, don't hit again
            float hitPeriod = 1 / hitsPerSecond;
            if (pastCollisionTimes.ContainsKey(col) && pastCollisionTimes[col] > Time.time - hitPeriod)
            {
                if (!(pastCollisionTimes[col] > Time.time - hitPeriod))
                    print("already hit!");
                continue;
            }
            
            var isEnemy = col.CompareTag("Enemy");
            var isBoss = col.CompareTag("Boss");
            if (isEnemy)
            {
                var enemy = col.GetComponent<EnemyTemplate>();
                if (enemy == null) throw new Exception("Object with tag `Enemy` did not have an `EnemyTemplate` script!");
                enemy.TakeDamage(this.damage);
            }
            else if (isBoss)
            {
                print("Hit boss!");
                BossHealth bossHealth = col.GetComponent<BossHealth>();
                bossHealth.Damage(damage);
            }

            if (isEnemy || isBoss)
            {
                pastCollisionTimes[col] = Time.time;
                onHit?.Invoke();
            }
        }
    }

    public void Setup(float newDamage, float newHitsPerSecond)
    {
        this.damage = newDamage;
        this.hitsPerSecond = newHitsPerSecond;
        this.isSetUp = true;
    }
}
