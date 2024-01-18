using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour {
    [SerializeField] float maxHealth = 100;

    float currentHealth;
    bool isDead = false;

    // NOTE: Assumes there's only one boss per scene!
    [SerializeField] DialoguePlayer postBossDialogue;
    
    [SerializeField, Tooltip("When the boss dies, this prefab is instantiated at it's position. Could be a dead sprite, particles, etc. Affects nothing if null.")]
    GameObject onDeathPrefab;

    private void Start() {
        currentHealth = maxHealth;
    }

    void Update() {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.O)) Damage(float.PositiveInfinity);
    }

    public void Damage(float damage) {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
            Die();
        }
        
        GetComponent<DamageFlash>().Damage();
        BossHealthBarUI.SetHealth(currentHealth / maxHealth);
    }

    public void Die() {
        isDead = true;
        print("BOSS DEFEATED!!");
        
        // turn off enemy spawning and destroy all current enemies
        FindObjectOfType<EnemySpawner>().enabled = false;
        foreach (var enemy in FindObjectsOfType<EnemyTemplate>())
            Destroy(enemy.gameObject);
        
        // destroy the boss
        this.gameObject.SetActive(false);
        if (onDeathPrefab != null) Instantiate(onDeathPrefab, transform.position, transform.rotation);

        // trigger post-boss dialogue
        postBossDialogue.gameObject.SetActive(true);

        // freeze weapons
        EquipmentManager.instance.Freeze();
        Player.instance.Freeze();
    }
}
