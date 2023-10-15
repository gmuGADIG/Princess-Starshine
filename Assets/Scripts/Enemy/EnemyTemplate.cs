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
using UnityEngine;

public class EnemyTemplate : MonoBehaviour
{
    #region VariableSettings
    [Header("Health Settings")]
    [SerializeField] 
    private float maxHealth = 100f;

    [Header("Character Settings")]
    [SerializeField] 
    private float movementSpeed = 5;

    [Tooltip("Where the enemy is moving too")]
    [SerializeField] 
    private GameObject moveTowardsObject;

    [Header("XP Settings")]
    [SerializeField] 
    private GameObject XPOrb;

    [SerializeField] 
    private float xpDropRadius = 3;

    [SerializeField] 
    private int xpDropAmount = 10;

    [Header("Sound Effects")]
    [Tooltip("When the enemy has taken damage")][SerializeField] AudioSource TakenDamageSoundEffect;
    #endregion

    private float currentHealth;
    private bool isDead;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float XPDropRadius { get => xpDropRadius; set => xpDropRadius = value; }
    public int XPDropAmount { get => xpDropAmount; set => xpDropAmount = value; }
    public bool IsDead {  get => isDead; set => isDead = value; }
    public void TakeDamage(float value) { 
        CurrentHealth -= value;
        TakenDamageSoundEffect.Play();
        CheckDeath();
    }

    public virtual void Die()
    {
        DistrubuteXP();
        Destroy(this.gameObject);
    }

    // TODO: Add code for enemy to move towards player
    public void MoveTowardsPlayer() {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveTowardsObject.transform.position, movementSpeed * Time.deltaTime);
    }

    public void CheckDeath() {
        if (CurrentHealth <= 0) { isDead = true;  }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        for (int i = 0; i <= xpDropAmount; i++)
        {
            /// COPY OBJECT OF XP ///
            GameObject orbObject = Instantiate(XPOrb);

            /// SET XP OBJECT'S POSITION TO THE SAME POSITION AS ENEMY ///
            orbObject.gameObject.transform.position = ENEMY_POSITION;
            
            /// GET A RANDOM POSITION AROUND THE ENEMY WITH A GIVEN RADIUS ///
            Vector3 position = orbObject.gameObject.transform.position;

            Vector3 topLeft = new Vector3(position.x - XPDropRadius, position.y + XPDropRadius, position.z);
            Vector3 bottomRight = new Vector3(position.x + XPDropRadius, position.y - XPDropRadius, position.z);

            float RandomX = UnityEngine.Random.Range(topLeft.x, bottomRight.x);
            float RandomY = UnityEngine.Random.Range(bottomRight.y, topLeft.y);
            Vector3 newPosition = new Vector3(RandomX, RandomY, 0) + position;

            /// SET XP ORB TO A RANDOM POSITION AROUND THE ENEMY ///
            orbObject.gameObject.transform.position = newPosition;
        }
    }

    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
        CheckDeath();
    }

    protected virtual void Update()
    {
        MoveTowardsPlayer();
        if (isDead) {
            Die();
        }
    }

}
