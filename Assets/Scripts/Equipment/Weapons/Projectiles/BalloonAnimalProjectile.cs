using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BalloonAnimalProjectile : Projectile {
    GameObject explosionPrefab;
    string explosionSoundName;
    float explosionSize;
    float explosionDamage;

    float inflationRate;
    float slowdownRate;

    float rotationalVelocity;
    SpriteRenderer sprite;

    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate) {
        base.Setup(weapon, target, damage, pierceCount, speed, knockback, size, dotRate);

        BalloonAnimal ba = (BalloonAnimal)weapon;
        maxLifeTime = ba.TimeUntilExplosion;
        explosionPrefab = ba.ExplosionPrefab;
        explosionSoundName = ba.ExplosionSoundName;
        explosionSize = ba.ExplosionSize;
        explosionDamage = ba.ExplosionDamage;
        inflationRate = ba.InflationRate;
        slowdownRate = speed * inflationRate;

        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        rotationalVelocity = Random.Range(-60, 60);

        LifetimeExpired += () => {
            SoundManager.Instance.PlaySoundGlobal(explosionSoundName);

            GameObject go = Instantiate(explosionPrefab);
            go.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                5
            );
            go.transform.rotation = sprite.transform.rotation;

            Projectile projectile = go.GetComponent<Projectile>();
            projectile.maxLifeTime = 0.3f;
            projectile.Setup(weapon, transform.position, explosionDamage, -1, 0, 0, this.size, 0);
            print($"Balloon animal's size is {this.size}");
        };
    }

    protected override void Update() {
        base.Update();
        UpdateSize();
        speed -= slowdownRate * Time.deltaTime;
        transform.localScale = new Vector3(size, size, 1);

        sprite.transform.rotation *= Quaternion.Euler(0, 0, rotationalVelocity * Time.deltaTime);
    }

    private void UpdateSize() {
        // set the intervals in which the balloon grows (numbers based on the inflating sound file)
        const float
            firstInflateStart = 0.5f,
            firstInflateEnd = 1f,
            secondInflateStart = 1.75f,
            secondInflateEnd = 2.25f;

        bool isInflating;
        if (timeAlive is >= firstInflateStart and < firstInflateEnd) isInflating = true;
        else if (timeAlive is >= secondInflateStart and < secondInflateEnd) isInflating = true;
        else isInflating = false;

        if (isInflating) size += inflationRate * Time.deltaTime;
    }
}
