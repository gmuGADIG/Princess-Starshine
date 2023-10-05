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
    [SerializeField] short maxHealth = 100;

    [Header("Character Settings")]
    [SerializeField] short movementSpeed = 5;
    [Tooltip("Where the enemy is moving too")][SerializeField] GameObject moveTowardsObject;

    [Header("XP Settings")]
    [SerializeField] GameObject XPOrb;
    [SerializeField] short XpDropRadius = 3;
    [SerializeField] short xpDropAmount = 10;

    [Header("Sound Effects")]
    [Tooltip("When the enemy has taken damage")][SerializeField] AudioSource TakenDamageSoundEffect;
    #endregion

    private short currentHealth;
    private bool isDead;

    #region Get-Set Health
    public short GetMaxHealth() {  return maxHealth; }
    public void SetMaxHealth(short value) { maxHealth = value; }
    #endregion
    #region Get-Set CurrentHealth
    public short GetCurrentHealth() { return currentHealth; }
    public void SetCurrentHealth(short value) {  currentHealth = value; }
    public void TakeDamage(short value) { 
        currentHealth -= value;
        TakenDamageSoundEffect.Play();
        CheckDeath();
    }
    #endregion
    #region Get-Set MovementSpeed
    public short GetMovementSpeed() { return movementSpeed; }
    public void SetMovementSpeed(short value) {  movementSpeed = value; }
    #endregion
    #region Get-Set XpDrop
    public short GetXPDropAmount() {  return xpDropAmount; }
    public void setXPDropAmount(short value) {  xpDropAmount = value; }
    #endregion

    // TODO: Add code for enemy to move towards player
    public void MoveTowardsPlayer() {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveTowardsObject.transform.position, movementSpeed * Time.deltaTime);
    }

    private void CheckDeath() {
        if (currentHealth <= 0) { isDead = true;  }
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
    private IEnumerator DistrubuteXP() {
        // Gettings position of enemy
        Vector3 ENEMY_POSITION = gameObject.transform.position;
        for (int i = 0; i <= xpDropAmount; i++)
        {
            /// COPY OBJECT OF XP ///
            GameObject @object = Instantiate(XPOrb);

            /// SET XP OBJECT'S POSITION TO THE SAME POSITION AS ENEMY ///
            @object.gameObject.transform.position = ENEMY_POSITION;
            
            /// GET A RANDOM POSITION AROUND THE ENEMY WITH A GIVEN RADIUS ///
            Vector3 position = @object.gameObject.transform.position;

            Vector3 topLeft = new Vector3(position.x - XpDropRadius, position.y + XpDropRadius, position.z);
            Vector3 bottomRight = new Vector3(position.x + XpDropRadius, position.y - XpDropRadius, position.z);

            float RandomX = UnityEngine.Random.Range(topLeft.x, bottomRight.x);
            float RandomY = UnityEngine.Random.Range(bottomRight.y, topLeft.y);
            Vector3 newPosition = new Vector3(RandomX, RandomY, 0) + position;

            /// SET XP ORB TO A RANDOM POSITION AROUND THE ENEMY ///
            @object.gameObject.transform.position = newPosition;
        }
        yield return null;
    }

    void Start()
    {
        currentHealth = maxHealth;
        CheckDeath();
    }

    private void Update()
    {
        MoveTowardsPlayer();
        if (isDead) {
            StartCoroutine(DistrubuteXP());
            Destroy(gameObject); // Gets rid of enemy from game.
        }
    }

}
