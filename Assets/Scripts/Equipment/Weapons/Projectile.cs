using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Basic projectile class. Extend it to add specific behavior. Modify it / add fields for common functionality.
 */
public class Projectile : MonoBehaviour
{
    /** After this many seconds, projectiles will be automatically removed. */
    [HideInInspector]
    public float maxLifeTime = float.PositiveInfinity;
    
    /** The weapon which fired this projectile. */
    protected ProjectileWeapon weapon;

    protected float speed;
    protected Vector2 velocity;
    protected float damage = 0;
    protected int pierceCount = 0;
    protected bool hasBeenSetUp = false;
    protected float knockback = 1;
    protected float size = 1;
    protected float timeAlive;
    protected float dotRate;

    public Action LifetimeExpired;
    internal ProjectileCollision projectileCollision;

    protected virtual void Start()
    {
        projectileCollision = GetComponent<ProjectileCollision>();
        if (projectileCollision != null)
        {
            projectileCollision.Setup(this.damage, this.dotRate, this.knockback);
            projectileCollision.onHit += OnProjectileHit;
        }
    
}

    protected virtual void OnProjectileHit()
    {
        if (pierceCount == 0) Destroy(this.gameObject); // < 0 is fine, because -1 pierce means infinite
        this.pierceCount -= 1;
    }

    protected virtual void Move() {
        transform.position += (Vector3) velocity * Time.deltaTime;
    }

    protected virtual void Update()
    {
        if (!hasBeenSetUp)
        {
            Destroy(this.gameObject);
            throw new Exception("Projectile has not been set up! Destroying projectile.");
        }
   
        Move(); // foobar

        timeAlive += Time.deltaTime;
        if (timeAlive > maxLifeTime)
        {
            Destroy(this.gameObject);
            LifetimeExpired?.Invoke();
        }
    }

    protected virtual void OnDestroy()
    {
        weapon.OnProjectileDestroy(this);
    }

    /**
     * Called by the weapon after creating the projectile (and after setting its position and place in the scene tree).
     */
    public virtual void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size, float dotRate)
    {
        timeAlive = 0;

        this.weapon = weapon;
        this.speed = speed;
        velocity = (target - (Vector2)this.transform.position).normalized * speed;
        this.pierceCount = pierceCount;
        this.damage = damage;
        this.knockback = knockback;
        this.size = size;
        this.transform.localScale = new Vector3(size, size);
        this.dotRate = dotRate;
        hasBeenSetUp = true;
    }

    /**
     * Does nothing by default!
     * Most weapons have no need to be changed when the weapon upgrades, as they'll quickly be replaced by new projectiles.
     * For long-lasting projectiles however, you'll need this to be implemented.
     */
    public virtual void OnWeaponLevelUp(float newDamage, int newPierceCount, float newSpeed, float newKnockback, float newSize, float newDotRate) { }
}
