using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

// NOTE: You probably don't want to override anything in ProjectileWeapon

/**
 * Specialized Weapon sub-class for weapons which shoot projectiles at a constant interval.
 * Properties of the weapon are visible in the inspector.
 * To add a specific weapon, open the EquipmentManager (in the Player prefab) in the inspector and add new weapons there.
 */
[Serializable]
abstract public class ProjectileWeapon : Weapon
{
    [Tooltip("Base stats of the weapon.")]
    [SerializeField] WeaponStats weaponStats;
    
    [Tooltip("Modifiers to the weapon's stats. For floats, 1 = +100% increase. For ints, 1 = +1 increase.")]
    protected WeaponStats statModifiers;
    
    [Tooltip("Modifiers which apply to all weapons.")]
    public static WeaponStats staticStatModifiers;
    
    
    #region Stat Properties
    protected float Damage => weaponStats.damage * (1 + statModifiers.damage) * (1 + staticStatModifiers.damage);
    protected float Knockback => weaponStats.knockback * (1 + statModifiers.knockback) * (1 + staticStatModifiers.knockback);
    protected float FireRate => weaponStats.fireRate * (1 + statModifiers.fireRate) * (1 + staticStatModifiers.fireRate);
    protected float DotRate => weaponStats.dotRate * (1 + statModifiers.dotRate) * (1 + staticStatModifiers.dotRate);
    protected float ProjectileSize => weaponStats.size * (1 + statModifiers.size) * (1 + staticStatModifiers.size);
    protected float ProjectileSpeed => weaponStats.projectileSpeed * (1 + statModifiers.projectileSpeed) * (1 + staticStatModifiers.projectileSpeed);
    protected int MaxProjectiles => weaponStats.maxProjectiles + statModifiers.maxProjectiles + staticStatModifiers.maxProjectiles;
    protected int ProjectilesPerShot => weaponStats.projectilesPerShot + statModifiers.projectilesPerShot + staticStatModifiers.projectilesPerShot;
    protected int PierceCount => weaponStats.pierceCount + statModifiers.pierceCount + staticStatModifiers.pierceCount;
    #endregion
    
    
    float timeUntilNextFire = 0.5f; // the 0.5 gives it a bit of time before it's first shot after equipping it

    [Tooltip("The strategy the weapon should use to determine its firing direction.")]
    [SerializeField] TargetType targetingStrategy;
    
    [Tooltip("If checked, the projectile will spawn at its target; otherwise, it emits from the player.")]
    [SerializeField] bool spawnProjectileAtTarget;
    
    [Tooltip("If checked, the projectile will be attached to the player and move with them.")]
    [SerializeField] bool projectileLocalSpace;

    [Tooltip("The object that gets initialized on each fire. Must have the <b>Projectile</b> component attached to it.")]
    [SerializeField] protected GameObject projectilePrefab;

    [Tooltip("The name of the sound played when this weapon is fired.")]
    [SerializeField] protected string shootSoundName;
    
    /** Set of active projectiles. Updated in Fire and OnProjectileDestroy. Necessary to update projectiles when the weapon levels up. */
    protected HashSet<Projectile> projectileSet = new();

    [Tooltip("Rotation offset (in degrees) of the projectiles.")]
    public float Spread = 0f;

    /**
     * Called whenever the weapon should fire, based on its `fireRate`.
     */
    protected virtual void Fire()
    {
        if (projectileSet.Count >= MaxProjectiles) return;
        
        void FireToTarget(Vector3 target) {
            var proj = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectileSet.Add(proj);

            Vector3 projectilePos = proj.transform.position;

            if (projectileLocalSpace) proj.transform.SetParent(EquipmentManager.instance.transform);
            if (spawnProjectileAtTarget) projectilePos = target;
            else projectilePos = EquipmentManager.instance.transform.position;

            proj.transform.position = new Vector3(
                projectilePos.x, projectilePos.y, proj.transform.position.z
            );

            proj.Setup(this, target, Damage, PierceCount, ProjectileSpeed, Knockback, ProjectileSize, DotRate);
            if (shootSoundName != "")
            {
                SoundManager.Instance.PlaySoundAtPosition(shootSoundName, new(), Camera.main.transform);
                //SoundManager.Instance.PlaySoundGlobal(shootSoundName);
            }
        }
        
        Vector3 playerPosition = Player.instance.transform.position;
        var targetPosition = (Vector3)GetTarget();
        // FireToTarget(targetPosition);
        Vector3 toTarget = targetPosition - playerPosition;
        for (int i = 0; i < ProjectilesPerShot; i++) {
            Vector3 newTarget = playerPosition + Quaternion.Euler(0,0, Random.Range(-Spread,Spread)) * toTarget;
            FireToTarget(newTarget);
        }
    }

    /** Called by the projectiles whenever one is destroyed. Used to remove from the set of projectiles. */
    public void OnProjectileDestroy(Projectile projectile)
    {
        projectileSet.Remove(projectile);
    }
    
    /// <summary>
    /// Gets a list of all the enemies currently alive.
    /// </summary>
    /// <returns>The list of enemies</returns>
    public static List<GameObject> getEnemies() {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Boss"));

        return enemies;
    }
    
    /**
     * Returns the position to fire towards in world coordinates.
     * For targeting strategies that aim at an enemy, this will be that enemy's position.
     * Otherwise, it will be the player's position plus the direction with an arbitrary magnitude.
     */
    
    Vector2 GetTarget()
    {
        var player = Player.instance;
        var enemies = getEnemies();
        
        if (targetingStrategy == TargetType.RandomDirection || enemies.Count == 0)
        {
            var randomRads = Random.Range(0, 2 * Mathf.PI);
            return (Vector2)player.transform.position + new Vector2(Mathf.Cos(randomRads), Mathf.Sin(randomRads));
        }

        switch (targetingStrategy)
        {
            case TargetType.WalkingDirection:
                return (Vector2) player.transform.position + player.facingDirection;
            case TargetType.RandomEnemy:
                return enemies[Random.Range(0, enemies.Count)].transform.position;
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
                proj.OnWeaponLevelUp(Damage, PierceCount, ProjectileSpeed, Knockback, ProjectileSize, DotRate);
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
            case WeaponLevelUpType.DotRate:
                statModifiers.dotRate += levelUp.amount;
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
    
    protected override object FreezeRaw() {
        return statModifiers;
    }

    protected override void Thaw(object data) {
        string type = data.GetType().ToString();

        statModifiers = (WeaponStats)data;
    }
}

public enum TargetType
{
    WalkingDirection, NearestEnemy, RandomEnemy, RandomDirection
}
