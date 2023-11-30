using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (isDead)
        {
            return;
        }
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        print("BOSS DEAD, LEVEL CLEAR, WHERE DO I GO FROM HERE?");
    }
}
