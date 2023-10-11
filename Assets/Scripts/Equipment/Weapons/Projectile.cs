using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 velocity;
    float damage = 0;
    int pierceCount = 0;
    bool hasBeenSetUp = false;
    float knockback = 1;
    float size = 1;

    void Update()
    {
        if (!hasBeenSetUp) throw new Exception("Projectile has not been set up!");
        transform.position += (Vector3) velocity * Time.deltaTime;
    }

    /**
     * Called by the weapon after creating the projectile (and after setting its position and place in the scene tree).
     */
    public void Setup(Vector2 target, float damage, int pierceCount, float speed, float knockback, float size)
    {
        velocity = (target - (Vector2)this.transform.position).normalized * speed;
        this.pierceCount = pierceCount;
        this.damage = damage;
        this.knockback = knockback;
        this.size = size;
        hasBeenSetUp = true;
    }
}
