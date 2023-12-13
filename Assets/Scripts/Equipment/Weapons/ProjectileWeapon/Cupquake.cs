using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cupquake : ProjectileWeapon {
    [Tooltip("The cupcake explosion size.")]
    public float CupquakeExplosionSize = 5;

    [Tooltip("How long the explosion is on screen.")]
    public float CupquakeExplosionLifetime = 0.3f;

    [Tooltip("The cupcake explosion prefab.")]
    public GameObject CupquakeExplosionPrefab;

    [Tooltip("The sound played when the falling cupcake explodes.")]
    public string CupquakeExplosionSoundName;

    [Tooltip("The falling cupcake will follow the target.")]
    public bool FollowTarget = false;
    // I'm overriding this method so I can do a custom handling of projectiles per shot
    protected override void Fire()
    {
        var enemies = ProjectileWeapon.getEnemies()
            .Where(enemy => TeaTime.pointInCameraBoundingBox(
                enemy.transform.position, 1, 1)).ToList();

        for (int i = 0; i < ProjectilesPerShot; i++) {
            if (enemies.Count == 0) {
                break;
            }
            // pick a random enemy
            var enemy = enemies[Random.Range(0, enemies.Count)];
            enemies.Remove(enemy);

            var proj = Instantiate(projectilePrefab).GetComponent<CupquakeProjectile>();
            proj.SetupExt(this, enemy, Damage, PierceCount, ProjectileSpeed, Knockback, ProjectileSize, DotRate, FollowTarget);
            projectileSet.Add(proj);

            if (shootSoundName != "")
            {
                SoundManager.Instance.PlaySoundGlobal(shootSoundName);
            }
        }
    }
}