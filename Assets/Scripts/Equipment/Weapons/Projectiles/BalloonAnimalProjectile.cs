using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BalloonAnimalProjectile : Projectile
{
    GameObject explosionPrefab;
    string explosionSoundName;
    float explosionSize;
    float explosionDamage;

    float inflationRate;
    float slowdownRate;

    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate)
    {
        base.Setup(weapon, target, damage, pierceCount, speed, knockback, size, dotRate);

        BalloonAnimal ba = (BalloonAnimal)weapon;
        maxLifeTime = ba.TimeUntilExplosion;
        explosionPrefab = ba.ExplosionPrefab;
        explosionSoundName = ba.ExplosionSoundName;
        explosionSize = ba.ExplosionSize;
        explosionDamage = ba.ExplosionDamage;
        inflationRate = ba.InflationRate;
        slowdownRate = speed * inflationRate;

        LifetimeExpired += () => {
            SoundManager.Instance.PlaySoundGlobal(explosionSoundName);

            GameObject go = Instantiate(explosionPrefab);
            go.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                5
            );
            go.transform.localScale = new Vector3(explosionSize, explosionSize);

            Projectile projectile = go.GetComponent<Projectile>();
            projectile.maxLifeTime = 0.3f;
            projectile.Setup(weapon, transform.position, explosionDamage, -1, 0, 0, explosionSize, 0);
        };
    }

    protected override void Update() {
        base.Update();
        size += inflationRate * Time.deltaTime;
        speed -= slowdownRate * Time.deltaTime;
        transform.localScale = new Vector3(size, size);
    }
}
