using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlassShoemerangProjectile : Projectile {
    float rotationalSpeed;

    float decelerationTime;
    float constantTime;

    Vector2 accel;

    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate)
    {
        base.Setup(weapon, target, damage, pierceCount, speed, knockback, size, dotRate);
        
        maxLifeTime = 10;
        
        var shoe = (GlassShoemerang)weapon;

        rotationalSpeed = shoe.RotationalSpeed / speed;

        decelerationTime = shoe.RoundaboutTime * shoe.RoundaboutRatio;
        constantTime = shoe.RoundaboutTime - decelerationTime;

        // v = v_0 + a_0t
        // 0 = v_0 + a_0t
        // -v_0 / t = a
        accel = -velocity / decelerationTime;
    }

    protected override void Update()
    {
        base.Update();

        transform.Rotate(Vector3.forward * rotationalSpeed * Time.deltaTime);

        if (constantTime > 0) {
            constantTime -= Time.deltaTime;
            return;
        }

        velocity += accel * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, speed);
    }
}
