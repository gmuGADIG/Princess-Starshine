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
    /** Times the weapon fires per second. If set to 0, the weapon will fire at the start and never again. */
    [SerializeField] float fireRate;
    float timeUntilNextFire = 0.5f; // the 0.5 gives it a bit of time before it's first shot after equipping it

    /** Amount of knockback to inflict on enemies hit by the projectiles. */
    [SerializeField] float knockback;

    /** Projectile size multiplier. Used for level-ups. */
    [SerializeField] float projectileSize;

    /** The amount of damage each projectile does. Exact details are left to the projectile script. */
    [SerializeField] float damage;

    /** Amount of projectiles to fire with each shot. */
    [SerializeField] int projectileCount;

    /** How fast the projectiles start out when fired. */
    [SerializeField] float projectileSpeed;

    /** Weapon will stop emitting once this amount of projectiles exist. */
    [SerializeField] int maxProjectiles = 1000;

    /** The amount of enemies the projectile can pierce through. 0 means destroy on first hit. -1 means infinite pierce. */
    [SerializeField] int pierceCount = -1;

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
    
    /** Set of active projectiles. Updated in Fire and OnProjectileDestroy. Necessary to update projectiles when the weapon levels up. */
    HashSet<Projectile> projectileSet = new();

    /**
     * Called whenever the weapon should fire, based on its `fireRate`.
     */
    private void Fire()
    {
        if (projectileSet.Count >= maxProjectiles) return;
        
        var proj = GameObject.Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectileSet.Add(proj);
        var targetPosition = GetTarget();
        if (projectileLocalSpace) proj.transform.SetParent(EquipmentManager.instance.transform);
        if (spawnProjectileAtTarget) proj.transform.position = targetPosition;
        else proj.transform.position = EquipmentManager.instance.transform.position;
        proj.Setup(this, targetPosition, damage, pierceCount, projectileSpeed, knockback, projectileSize);
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
            timeUntilNextFire += 1f / fireRate; // infinity if fireRate is 0
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
            this.levelUpsDone += 1;
            
            foreach (var proj in projectileSet)
                proj.OnWeaponLevelUp(damage, pierceCount, projectileSpeed, knockback, projectileSize);
        };

        return (description, onApply);
    }

    public void ApplyLevelUp(WeaponLevelUp levelUp)
    {
        switch (levelUp.type)
        {
            case WeaponLevelUpType.Damage:
                damage *= levelUp.amount;
                break;
            case WeaponLevelUpType.KnockBack:
                knockback *= levelUp.amount;
                break;
            case WeaponLevelUpType.Pierce:
                pierceCount += (int)levelUp.amount;
                break;
            case WeaponLevelUpType.FireRate:
                fireRate *= levelUp.amount;
                break;
            case WeaponLevelUpType.AoESize:
                projectileSize *= levelUp.amount;
                break;
            case WeaponLevelUpType.ProjectileCount:
                projectileCount += (int)levelUp.amount;
                break;
            case WeaponLevelUpType.ProjectileSize:
                projectileSize *= levelUp.amount;
                break;
            case WeaponLevelUpType.ProjectileSpeed:
                projectileSpeed *= levelUp.amount;
                break;
            case WeaponLevelUpType.MaxProjectiles:
                maxProjectiles += (int)levelUp.amount;
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