using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.LookDev;
using Random = UnityEngine.Random;

public class MargaritaThePomeranian : Weapon {
    [Tooltip("Base stats of the weapon.")]
    [SerializeField] WeaponStats weaponStats;

    [Tooltip("Modifiers to the weapon's stats. For floats, 1 = +100% increase. For ints, 1 = +1 increase.")]
    WeaponStats statModifiers;
    
    #region Stat Properties
    float Damage => weaponStats.damage * (1 + statModifiers.damage) * (1 + ProjectileWeapon.staticStatModifiers.damage);
    float Knockback => weaponStats.knockback * (1 + statModifiers.knockback) * (1 + ProjectileWeapon.staticStatModifiers.knockback);
    float FireRate => weaponStats.fireRate * (1 + statModifiers.fireRate) * (1 + ProjectileWeapon.staticStatModifiers.fireRate);
    float DotRate => weaponStats.dotRate * (1 + statModifiers.dotRate) * (1 + ProjectileWeapon.staticStatModifiers.dotRate);
    float ProjectileSize => weaponStats.size * (1 + statModifiers.size) * (1 + ProjectileWeapon.staticStatModifiers.size);
    float ProjectileSpeed => weaponStats.projectileSpeed * (1 + statModifiers.projectileSpeed) * (1 + ProjectileWeapon.staticStatModifiers.projectileSpeed);
    int MaxProjectiles => weaponStats.maxProjectiles + statModifiers.maxProjectiles + ProjectileWeapon.staticStatModifiers.maxProjectiles;
    int ProjectilesPerShot => weaponStats.projectilesPerShot + statModifiers.projectilesPerShot + ProjectileWeapon.staticStatModifiers.projectilesPerShot;
    int PierceCount => weaponStats.pierceCount + statModifiers.pierceCount + ProjectileWeapon.staticStatModifiers.pierceCount;
    #endregion

    public GameObject PomeranianPrefab;
    public GameObject BarkPrefab;
    public string BarkSoundName;
    Pomeranian pomeranian;

    public override void OnEquip() {
        if (Player.instance == null) { return; }
        if (pomeranian != null) { return; }

        var go = Instantiate(PomeranianPrefab, Player.instance.transform.position, Quaternion.identity);
        pomeranian = go.GetComponent<Pomeranian>();
        Assert.IsNotNull(pomeranian);

        pomeranian.Speed = ProjectileSpeed;
    }

    void OnEnable() {
        if (pomeranian == null) {
            OnEquip();
        }
    }

    void OnDisable() {
        if (pomeranian == null) { return; }
        Destroy(pomeranian.gameObject);
        pomeranian = null;
    }

    public override void OnUnEquip() { }

    float barkTimer = 0f;
    public override void Update() {
        barkTimer -= Time.deltaTime;

        if (barkTimer <= 0) {
            bool collision = Physics2D.CircleCastAll(
                pomeranian.transform.position,
                ProjectileSize * .8f,
                Vector2.zero,
                0
            ).Where(rh => 
                rh.collider.gameObject.GetComponent<EnemyTemplate>() != null
                || rh.collider.gameObject.GetComponent<BossHealth>() != null
            )
            .Take(1).Any();

            if (collision) {
                // spawn a projectile
                var go = Instantiate(BarkPrefab, pomeranian.transform.position, Quaternion.identity);
                go.transform.position = new Vector3(
                    go.transform.position.x,
                    go.transform.position.y,
                    10                    
                );
                var projectile = go.GetComponent<ProjectileCollision>();
                projectile.Setup(Damage, 0, Knockback);
                projectile.transform.localScale = new Vector2(ProjectileSize, ProjectileSize);

                // reset bark timer
                barkTimer = 1 / FireRate;

                // woof
                SoundManager.Instance.PlaySoundGlobal(BarkSoundName);
            }
        }
    }

    public override void ProcessOther(Equipment other)
    {
        // TODO: handle synergy
        PostLevelUp(); // handle the fact that some passives might give a static buff
    }

    public override void ProcessOtherRemoval(Equipment other)
    {
        // TODO: handle undoing synergy
    }

    string StringOfWeaponLevelUp(WeaponLevelUp levelUp) {
        switch (levelUp.type) {
            case WeaponLevelUpType.FireRate:
                return $"+{levelUp.amount:P0} Bark Rate";
            case WeaponLevelUpType.ProjectileSpeed:
                return $"+{levelUp.amount:P0} Move Speed";
            case WeaponLevelUpType.ProjectileSize:
                return $"+{levelUp.amount:P0} Bark Size";
            default:
                return levelUp.ToString();
        }
    }

    public override (string description, Action onApply) GetLevelUps()
    {
        // shuffle levelUpOptions and get the first 2 (definitely not an optimal shuffle algorithm, but a simple and good-enough one)
        var levelUps = levelUpOptions.OrderBy(_ => Random.Range(0f, 1f)).Take(2).ToArray();
        var description =
             "Weapon Level Up!\n" +
            $"{StringOfWeaponLevelUp(levelUps[0])}\n" +
            $"{StringOfWeaponLevelUp(levelUps[1])}";
        Action onApply = () =>
        {
            foreach (var levelUp in levelUps)
                ApplyLevelUp(levelUp);
        };

        return (description, onApply);
    }

    void PostLevelUp() {
        if (pomeranian != null) {
            pomeranian.Speed = ProjectileSpeed;
        }

        barkTimer = Mathf.Min(barkTimer, 1 / FireRate);
    }

    public void ApplyLevelUp(WeaponLevelUp levelUp)
    {
        switch (levelUp.type)
        {
            case WeaponLevelUpType.Damage:
                statModifiers.damage += levelUp.amount;
                break;
            case WeaponLevelUpType.Knockback:
                statModifiers.knockback += levelUp.amount;
                break;
            case WeaponLevelUpType.Pierce:
                statModifiers.pierceCount += (int) levelUp.amount;
                break;
            case WeaponLevelUpType.FireRate:
                statModifiers.fireRate += levelUp.amount;
                break;
            case WeaponLevelUpType.ProjectilesPerShot:
                statModifiers.projectilesPerShot += (int) levelUp.amount;
                break;
            case WeaponLevelUpType.ProjectileSize:
                statModifiers.size += levelUp.amount;
                break;
            case WeaponLevelUpType.ProjectileSpeed:
                statModifiers.projectileSpeed += levelUp.amount;
                break;
            case WeaponLevelUpType.MaxProjectiles:
                statModifiers.maxProjectiles += (int) levelUp.amount;
                break;
            default:
                throw new Exception($"Invalid weapon level-up type! type = {levelUp.type}");
        }
        PostLevelUp();
    }

    protected override object FreezeRaw() {
        return statModifiers;
    }

    protected override void Thaw(object data) {
        statModifiers = (WeaponStats)data;

        OnEquip();
        PostLevelUp();
    }
}

