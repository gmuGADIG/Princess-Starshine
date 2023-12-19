using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;

    float currentHealth;
    bool isDead = false;

    // NOTE: Assumes there's only one boss per scene!
    public static Action BossDied; 

    private void Start()
    {
        currentHealth = maxHealth;
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
        BossDied?.Invoke();
        print("BOSS DEFEATED!!");
        SceneManager.LoadScene("Scenes/Build Scenes/LevelPreview");
    }
}
