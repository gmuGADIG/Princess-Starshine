using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Specialized Weapon sub-class for weapons which shoot projectiles at a constant interval.
 * Properties of the weapon are visible in the inspector.
 * To add a specific weapon, open the EquipmentManager (in the Player prefab) in the inspector and add new weapons there.
 */
[Serializable]
public sealed class ProjectileWeapon : Weapon
{
    [Tooltip("Base stats of the weapon.")]
    [SerializeField] WeaponStats weaponStats;
    
    [Tooltip("Modifiers to the weapon's stats. For floats, 1 = +100% increase. For ints, 1 = +1 increase.")]
    WeaponStats statModifiers;
    
    [Tooltip("Modifiers which apply to all weapons.")]
    public static WeaponStats staticStatModifiers;
    
    
    #region Stat Properties
    float Damage => weaponStats.damage * (1 + statModifiers.damage) * (1 + staticStatModifiers.damage);
    float Knockback => weaponStats.knockback * (1 + statModifiers.knockback) * (1 + staticStatModifiers.knockback);
    float FireRate => weaponStats.fireRate * (1 + statModifiers.fireRate) * (1 + staticStatModifiers.fireRate);
    float ProjectileSize => weaponStats.size * (1 + statModifiers.size) * (1 + staticStatModifiers.size);
    float ProjectileSpeed => weaponStats.projectileSpeed * (1 + statModifiers.projectileSpeed) * (1 + staticStatModifiers.projectileSpeed);
    int MaxProjectiles => weaponStats.maxProjectiles + statModifiers.maxProjectiles + staticStatModifiers.maxProjectiles;
    int ProjectilesPerShot => weaponStats.projectilesPerShot + statModifiers.projectilesPerShot + staticStatModifiers.projectilesPerShot;
    int PierceCount => weaponStats.pierceCount + statModifiers.pierceCount + staticStatModifiers.pierceCount;
    #endregion
    
    
    float timeUntilNextFire = 0.5f; // the 0.5 gives it a bit of time before it's first shot after equipping it
    
    /** How the weapon should determine which direction to fire in */
    [SerializeField] TargetType targetingStrategy;

    /** True if the projectile should spawn at its target, as opposed to emitting from the player. */
    [SerializeField] bool spawnProjectileAtTarget;

    /** True if the projectile should be attached to the player and move with them. Otherwise, it operates in world space and moves independently of the player. */
    [SerializeField] bool projectileLocalSpace;

    /**
     * The object that gets initialized on each fire.
     * Object must have the Projectile component attached to it.
     */
    [SerializeField] GameObject projectilePrefab;

    [Tooltip("The name of the sound played when this weapon is fired.")]
    [SerializeField] string shootSoundName;
    
    /** Set of active projectiles. Updated in Fire and OnProjectileDestroy. Necessary to update projectiles when the weapon levels up. */
    HashSet<Projectile> projectileSet = new();

    /**
     * Called whenever the weapon should fire, based on its `fireRate`.
     */
    private void Fire()
    {
        if (projectileSet.Count >= MaxProjectiles) return;
        
        var proj = GameObject.Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectileSet.Add(proj);
        var targetPosition = GetTarget();
        if (projectileLocalSpace) proj.transform.SetParent(EquipmentManager.instance.transform);
        if (spawnProjectileAtTarget) proj.transform.position = targetPosition;
        else proj.transform.position = EquipmentManager.instance.transform.position;
        proj.Setup(this, targetPosition, Damage, PierceCount, ProjectileSpeed, Knockback, ProjectileSize);
        if (shootSoundName != "")
        {
            SoundManager.Instance.PlaySoundGlobal(shootSoundName);
        }
        // TODO: handle projectile count
        // basic cases can be handled by just looping this, but if they have the same target, they'll need to be separated a bit
    }

    /** Called by the projectiles whenever one is destroyed. Used to remove from the set of projectiles. */
    public void OnProjectileDestroy(Projectile projectile)
    {
        projectileSet.Remove(projectile);
    }
    
    /**
     * Returns the position to fire towards in world coordinates.
     * For targeting strategies that aim at an enemy, this will be that enemy's position.
     * Otherwise, it will be the player's position plus the direction with an arbitrary magnitude.
     */
    Vector2 GetTarget()
    {
        var player = Player.instance;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (targetingStrategy == TargetType.RandomDirection || enemies.Length == 0)
        {
            var randomRads = Random.Range(0, 2 * Mathf.PI);
            return (Vector2)player.transform.position + new Vector2(Mathf.Cos(randomRads), Mathf.Sin(randomRads));
        }

        switch (targetingStrategy)
        {
            case TargetType.WalkingDirection:
                // TODO: handle stationary player better (remember most recent walking direction?)
                return (Vector2) player.transform.position + player.velocity;
            case TargetType.RandomEnemy:
                return enemies[Random.Range(0, enemies.Length)].transform.position;
            case TargetType.NearestEnemy:
                var min = new Vector2(0, 0);
                var minDist = 1000000f;
                foreach (var enemy in enemies)
                {
                    var dist = Vector2.Distance(player.transform.position, enemy.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        min = enemy.transform.position;
                    }
                }
                return min;
            case TargetType.RandomDirection:
                var randomRads = Random.Range(0, 2 * Mathf.PI);
                return (Vector2) player.transform.position + new Vector2(Mathf.Cos(randomRads), Mathf.Sin(randomRads));
            default:
                throw new Exception($"Invalid targeting strategy `{targetingStrategy}`!");
        }
    }
    
    public override void OnEquip() { }
    public override void OnUnEquip() { }

    public override void Update()
    {
        timeUntilNextFire -= Time.deltaTime;
        if (timeUntilNextFire <= 0)
        {
            Fire();
            timeUntilNextFire += 1f / FireRate    ; // infinity if fireRate is 0
        }
    }

    public override void ProcessOther(Equipment other)
    {
        // TODO: handle synergy
    }

    public override void ProcessOtherRemoval(Equipment other)
    {
        // TODO: handle undoing synergy
    }

    public override (string description, Action onApply) GetLevelUps()
    {
        // shuffle levelUpOptions and get the first 2 (definitely not an optimal shuffle algorithm, but a simple and good-enough one)
        var levelUps = levelUpOptions.OrderBy(_ => Random.Range(0f, 1f)).Take(2).ToArray();
        var description =
             "Weapon Level Up!\n" +
            $"{levelUps[0]}\n" +
            $"{levelUps[1]}";
        Action onApply = () =>
        {
            foreach (var levelUp in levelUps)
                ApplyLevelUp(levelUp);
            
            foreach (var proj in projectileSet)
                proj.OnWeaponLevelUp(Damage, PierceCount, ProjectileSpeed, Knockback, ProjectileSize);
        };

        return (description, onApply);
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
    }
}

public enum TargetType
{
    WalkingDirection, NearestEnemy, RandomEnemy, RandomDirection
}