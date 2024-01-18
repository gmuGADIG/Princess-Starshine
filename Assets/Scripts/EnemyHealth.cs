using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    //the enemy health
    [SerializeField] private float health = 10f;

    //the current enemy health
    private float currentHealth;

    private void Start() {
        //initializes the current health
        currentHealth = health;
    }

    void Update() {
        //destroys the enemy if health is at or below zero
        if(currentHealth <= 0) {
            Destroy(gameObject);
        }

        //limits the enemy health
        if(currentHealth > health) {
            currentHealth = health;
        }
    }

    //decrement enemy health by given damage
    public void decrementHealth(float damage) {
        currentHealth -= damage;
    }

    //increment enemy health by given heal amount
    public void incrementHealth(float heal) {
        currentHealth += heal;
    }
}
