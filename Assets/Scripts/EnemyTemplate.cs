/*
 * Author: ServerDaLite (Dylan H.)
 * Date: 09/21/23
 * Tasks needing to complete this 'task'
 *  - Make enemy lose health when taking damage ( We could just do this with the weapon )
 *  - Add XP to player ( We need the player XP variable )
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

    [Header("Drops")]
    [SerializeField] short xpDropAmount = 10;

    [Header("Character Settings")]
    [SerializeField] short movementSpeed = 5;
    [Tooltip("Where the enemy is moving too")][SerializeField] GameObject moveTowardsObject;

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
    public void TakeDamage(short value) { currentHealth -= value;  }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) {
            // TODO : Make enemy take damage from the damage amount from bullet
            TakenDamageSoundEffect.Play();
            CheckDeath();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        MoveTowardsPlayer();
        if (isDead) {
            Destroy(gameObject); // Gets rid of enemy from game.
        }
    }

}
