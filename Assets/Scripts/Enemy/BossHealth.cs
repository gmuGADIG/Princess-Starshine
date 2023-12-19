using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;

    float currentHealth;
    bool isDead = false;

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
        print("BOSS DEFEATED!!");
    
        //may have to change the number based on index of the first level
        if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene("Scenes/DailogueAfterBoss");
        else
            SceneManager.LoadScene("Scenes/Build Scenes/LevelPreview");
    }
}
