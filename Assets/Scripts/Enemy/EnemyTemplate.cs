/*
 * Author: ServerDaLite (Dylan H.)
 * Date: 09/21/23
 * Tasks needing to complete this 'task'
 *  - Make enemy lose health when taking damage ( We could just do this with the weapon )
 *  - Add XP to player ( We need the player XP variable )
 *  
 *  
 *  Damage will be checked on the bullet script side.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class EnemyTemplate : MonoBehaviour {
    #region VariableSettings
    [Header("Health Settings")]
    [SerializeField] 
    private float maxHealth = 100f;

    [Header("Character Settings")]
    [SerializeField] 
    private float movementSpeed = 5;

    [Tooltip("Where the enemy is moving too")]
    [SerializeField] 
    protected GameObject moveTowardsObject;

    [Tooltip("How powerful knockback is on the enemy.")]
    [SerializeField] 
    protected float knockbackMultiplier = 1f;

    [Tooltip("How long knockback lasts on the enemy.")]
    [SerializeField] 
    protected float knockbackDuration = .2f;

    [Header("XP Settings")]
    [SerializeField] 
    private GameObject XPOrb;

    private const float xpDropRadius = 1.5f;

    [SerializeField] 
    private int xpDropAmount = 3;

    [Header("Sound Effects")]
    [Tooltip("Name of sound played on death")]
    [SerializeField] string deathSoundName;
    #endregion

    private float currentHealth;
    private bool isDead;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; protected set => currentHealth = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public int XPDropAmount { get => xpDropAmount; set => xpDropAmount = value; }
    public bool IsDead {  get => isDead; set => isDead = value; }

    private Rigidbody2D rb;
    public Rigidbody2D RigidBody { get => rb; }

    private Vector3 knockbackVelocity;
    private float knockbackDurationLeft = 0f;

    public static Action EnemyDied;

    /**
     * Called when the enemy has knockback applied
     **/
    public void ApplyKnockback(Vector2 _knockbackVelocity) {
        knockbackVelocity = _knockbackVelocity;
        knockbackDurationLeft = knockbackDuration;
    }
    
    /**
     * Called when the enemy takes damage
     * Default: Decreases the current health by the given value, plays the taken damage sound effect, and checks if the enemy is dead
     **/
    public void TakeDamage(float value) { 

        CurrentHealth -= value;
        CheckDeath();

        GetComponent<DamageFlash>().Damage();
    }
    
    /* 
     * Called when the enemy should die
     * Default: Destroys the object and distributes XP
     */
    public virtual void Die() {
        if (isDead) { return; }
        Destroy(gameObject);
        EnemyDied?.Invoke();
        isDead = true;
        EnemyManager.enemyManager.enemies.Remove(gameObject);
        DistrubuteXP();
        SoundManager.Instance.PlaySoundAtPosition(deathSoundName, transform.position);
        //StartCoroutine(DelayedDestroy());
    }

    protected void MoveTowardsObject() {
        if (knockbackDurationLeft > 0) {
            gameObject.transform.position += knockbackVelocity * Time.deltaTime * knockbackMultiplier;
            knockbackDurationLeft -= Time.deltaTime;
        } else { MoveBehavior(); }
    }

    protected virtual void MoveBehavior() {
        if (moveTowardsObject != null) {
            var startPos = (Vector2)transform.position;
            var endPos = (Vector2)moveTowardsObject.transform.position;
            var resultPos = Vector2.MoveTowards(startPos, endPos, movementSpeed * Time.deltaTime);

            transform.position = new Vector3(
                resultPos.x, resultPos.y, transform.position.z
            );
        }
    }

    protected void CheckDeath() {
        if (CurrentHealth <= 0) { 
            Die();
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet")) {
            
            TakenDamageSoundEffect.Play();
            CheckDeath();
        }
    } */

    /*
     * Copy object of XP
     * Set XP position to the enemy position
     * Get a random position around the enemy with a given radius
     * Set XP orb to that random position around the enemy
     */
    private void DistrubuteXP() {
        // Gettings position of enemy
        Vector3 ENEMY_POSITION = gameObject.transform.position;
        for (int i = 1; i <= xpDropAmount; i++) {
            var orb = Instantiate(XPOrb);
            orb.transform.position = ENEMY_POSITION + (Vector3)Random.insideUnitCircle * xpDropRadius;

            // /// SET XP OBJECT'S POSITION TO THE SAME POSITION AS ENEMY ///
            // orbObject.gameObject.transform.position = ENEMY_POSITION;
            
            // /// GET A RANDOM POSITION AROUND THE ENEMY WITH A GIVEN RADIUS ///
            // Vector3 position = orbObject.gameObject.transform.position;

            // Vector3 topLeft = new Vector3(position.x - XPDropRadius, position.y + XPDropRadius, position.z);
            // Vector3 bottomRight = new Vector3(position.x + XPDropRadius, position.y - XPDropRadius, position.z);
            //
            // float RandomX = UnityEngine.Random.Range(topLeft.x, bottomRight.x);
            // float RandomY = UnityEngine.Random.Range(bottomRight.y, topLeft.y);
            // Vector3 newPosition = new Vector3(RandomX, RandomY, 0) + position;
            //
            // /// SET XP ORB TO A RANDOM POSITION AROUND THE ENEMY ///
            // orbObject.gameObject.transform.position = newPosition;
        }
    }

    /**
     * Called when the object is created
     * Default: Sets the current health to the max health, and checks if the enemy is dead
     */
    protected virtual void Start() {
        if (moveTowardsObject == null) { moveTowardsObject = GameObject.FindGameObjectWithTag("Player"); }
        if (XPOrb == null) { XPOrb = GameObject.FindGameObjectWithTag("XPOrb"); }
        if (tag.CompareTo("") == 0) { tag = "Enemy"; }
        CurrentHealth = MaxHealth;
        CheckDeath();
        EnemyManager.enemyManager.enemies.Add(gameObject);
    }

    /**
     * Called Every Frame
     * Default: Moves towards the object and handles collisions
     **/
    protected virtual void Update() {
        MoveTowardsObject();
    }


    IEnumerator DelayedDestroy() {
        yield return new WaitForSeconds(.05f);
        Destroy(gameObject);
    }
}
