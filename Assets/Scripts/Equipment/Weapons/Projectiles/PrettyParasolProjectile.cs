using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrettyParasolProjectile : Projectile
{
    [Tooltip("How long the projectile lasts.")]
    public float lifetime = 0.3f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        maxLifeTime = lifetime;
    }

    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate)
    {
        base.Setup(weapon, target, damage, pierceCount, speed, knockback, size, dotRate);

        transform.position = Player.instance.transform.position + (Vector3)Player.instance.facingDirection * 2;
        transform.rotation = Quaternion.FromToRotation(Vector2.right, Player.instance.facingDirection);
    }
}
