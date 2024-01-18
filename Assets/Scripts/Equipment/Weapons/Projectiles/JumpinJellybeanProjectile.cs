using UnityEngine;

public class JumpinJellybeanProjectile : Projectile {
    string bounceSoundName;

    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate) {
        base.Setup(weapon, target, damage, pierceCount, speed, knockback, size, dotRate);
        bounceSoundName = ((JumpinJellybeans)weapon).BounceSoundName;
    }

    protected override void Update() {
        base.Update();
    
        if (!TeaTime.cameraBoundingBox().Contains(transform.position)) {
            Destroy(gameObject);
        }
    }

    protected override void OnProjectileHit() {
        base.OnProjectileHit();
        float randomRads = Random.Range(0, 2 * Mathf.PI);
        velocity = new Vector2(Mathf.Cos(randomRads), Mathf.Sin(randomRads)) * speed;

        SoundManager.Instance.PlaySoundGlobal(bounceSoundName);
    }
}
