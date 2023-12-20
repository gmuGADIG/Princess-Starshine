using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;

    float currentHealth;
    bool isDead = false;

    // NOTE: Assumes there's only one boss per scene!
    [SerializeField] DialoguePlayer postBossDialogue;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) Damage(100);
    }

    public void Damage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        
        GetComponent<DamageFlash>().Damage();
    }

    public void Die()
    {
        isDead = true;
        print("BOSS DEFEATED!!");
        
        // turn off enemy spawning and destroy all current enemies
        FindObjectOfType<EnemySpawner>().enabled = false;
        foreach (var enemy in FindObjectsOfType<EnemyTemplate>())
            Destroy(enemy.gameObject);
        
        // destroy the boss
        // todo: add support for animations
        this.gameObject.SetActive(false);
        
        // trigger post-boss dialogue
        postBossDialogue.gameObject.SetActive(true);
    }
}