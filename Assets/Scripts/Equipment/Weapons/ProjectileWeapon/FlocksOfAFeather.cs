using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlocksOfAFeather : ProjectileWeapon {
    [Tooltip("How far away the projectiles spin around the player.")]
    public float OrbitRadius = 1f;

    public override void ProcessOther(Equipment other) {
        print("processother");
        base.ProcessOther(other);
        ApplyLevelUp(null);
    }

    bool shouldSound = true;

    public override void ApplyLevelUp(WeaponLevelUp? levelUp) {
        print("applylevelup");
        // apply the level up
        if (levelUp != null) {
            shouldSound = true;
            base.ApplyLevelUp(levelUp);
        }

        // clean up
        foreach (var proj in projectileSet) {
            Destroy(proj.gameObject);
        }

        // then respawn the bullets
        Fire();
    }

    protected override void Fire() {
        var increment = 2 * Mathf.PI / ProjectilesPerShot;
        
        var angle = 0f;
        while (angle < 2 * Mathf.PI) {
            // instance the projectile in local space at (cos(angle), sin(angle), 0) * radius
            var proj = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectileSet.Add(proj);
            
            proj.transform.SetParent(Player.instance.transform);
            proj.transform.localPosition = 
                new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * OrbitRadius;

            proj.Setup(this, new(), Damage, PierceCount, ProjectileSpeed, Knockback, ProjectileSize, DotRate);

            // NOTE: No sound!
            angle += increment;
        }

        if (shouldSound) {
            shouldSound = false;
            SoundManager.Instance.PlaySoundGlobal(shootSoundName);
        }
    }
}
