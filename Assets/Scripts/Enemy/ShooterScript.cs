using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : EnemyTemplate {
    [Header("Shooter Properties")]
    [Tooltip("The projectile that the enemy shoots.")]
    public GameObject projectilePrefab;

    [Tooltip("The speed the bullet travels at.")]
    public float projectileSpeed = 30f;

    [Tooltip("How long the bullet lives for.")]
    public float lifeTime = 3f;

    [Tooltip("Time between enemy shots.")]
    public float fireTimer = 1f;

    [Tooltip("How much damage the bullet will do.")]
    public float bulletDamage = 2f;

    [Tooltip("How close the enemy will get to the player.")]
    public float perferredPlayerDistance = 4f;

    float initialFireTimer;

    new SpriteRenderer renderer;

    protected override void Start() {
        base.Start();

        initialFireTimer = fireTimer;
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        // Only shoot if the enemy is on screen
        if (!renderer.isVisible) { return; }

        fireTimer -= 1 * Time.deltaTime;
        if (fireTimer <= 0)
        {
            Fire();
            fireTimer = initialFireTimer;
        }
    }

    protected override void MoveBehavior()
    {
        // If the player is far
        if ((Player.instance.transform.position - transform.position).magnitude > perferredPlayerDistance) {
            // Walk towards it
            base.MoveBehavior();
        } else { // If the player is close
            if (moveTowardsObject != null) {
                // Walk away from it
                var startPos = (Vector2)transform.position;
                var endPos = (Vector2)moveTowardsObject.transform.position;
                var resultPos = Vector2.MoveTowards(startPos, endPos, -MovementSpeed * Time.deltaTime);

                transform.position = new Vector3(
                    resultPos.x, resultPos.y, transform.position.z
                );
            }
        }
    }

    public void Fire()
        { 
        // Create an enemy bullet where the enemy stands
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity)
            .GetComponent<EnemyBullet>();

        // Setup the bullet
        var toPlayerHat = ((Vector2)(Player.instance.transform.position - transform.position)).normalized;
        projectile.Setup(toPlayerHat * projectileSpeed, lifeTime, bulletDamage);
    }
}
