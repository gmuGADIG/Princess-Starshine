using System;
using UnityEngine;

interface ICupquakeTarget {
    bool MetTarget(CupquakeProjectile projectile);
    void RenderLine(CupquakeProjectile projectile, LineRenderer line);
}

class CTPosition : ICupquakeTarget
{
    Vector3 target;
    public CTPosition(Vector3 target) {
        this.target = target;
    }

    public bool MetTarget(CupquakeProjectile projectile) {
        return projectile.transform.position.y <= target.y;
    }

    public void RenderLine(CupquakeProjectile projectile, LineRenderer line) {
        line.positionCount = 2;
        line.SetPositions(new Vector3[] { projectile.transform.position, target });
    }
}
class CTGameObject : ICupquakeTarget
{
    GameObject target;

    public CTGameObject(GameObject target) {
        this.target = target;
    }

    public bool MetTarget(CupquakeProjectile projectile) {
        return projectile.transform.position.y <= target.transform.position.y;
    }

    public void RenderLine(CupquakeProjectile projectile, LineRenderer line) {
        line.positionCount = 2;
        line.SetPositions(new Vector3[] { projectile.transform.position, target.transform.position });
    }
}

class CupquakeProjectile : Projectile {
    ICupquakeTarget target;
    GameObject explosionPrefab;
    string explosionSoundName;
    float explosionSize;
    float explosionLifeTime;
    LineRenderer line;

    Animator animator;

    bool alt;

    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate)
    {
        // use CupquakeProjectile.SetupExt
        throw new NotSupportedException();
    }

    // no i will not use overloads, i am a rust programmer
    public void SetupExt(
        Cupquake weapon, GameObject target, float damage, int pierceCount, float speed,
        float knockback, float size, float dotRate, bool followTarget
    ) {
        base.Setup(weapon, target.transform.position, damage, pierceCount, speed, knockback, size, dotRate);

        alt = UnityEngine.Random.Range(0f, 1f) < 0.5;
        Debug.Assert(TryGetComponent(out animator));
        animator.SetBool("Alt", alt);

        if (followTarget) {
            this.target = new CTGameObject(target);
            transform.SetParent(target.transform);
        } else {
            this.target = new CTPosition(target.transform.position);
        }

        // we position ourselves above the target, then when we reach the target, boom
        float height = TeaTime.cameraBoundingBox().height;
        transform.position = new Vector3(
            target.transform.position.x,
            target.transform.position.y + height,
            transform.position.z
        );

        explosionPrefab = weapon.CupquakeExplosionPrefab;
        explosionSoundName = weapon.CupquakeExplosionSoundName;
        explosionSize = weapon.CupquakeExplosionSize;
        explosionLifeTime = weapon.CupquakeExplosionLifetime;

        line = GetComponentInChildren<LineRenderer>(true);
    }

    protected override void Move()
    {
        // we just want to move down
        transform.position += Vector3.down * speed * Time.deltaTime;

        // and also render lines ig
        target.RenderLine(this, line);
        line.enabled = true; // hide the line on the first frame to avoid the weird flicker


        if (target.MetTarget(this)) {
            // boom!
            Destroy(gameObject);

            var explosion = Instantiate(explosionPrefab).GetComponent<CupquakeExplosion>();
            explosion.transform.position = transform.position;
            explosion.maxLifeTime = explosionLifeTime;
            explosion.SetupExt(weapon, Vector2.zero, damage, pierceCount, 0, knockback, size * explosionSize, dotRate, alt);

            SoundManager.Instance.PlaySoundGlobal(explosionSoundName);
        }
    }
}