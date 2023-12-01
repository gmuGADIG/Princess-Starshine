using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The player's max health.")]
    public float maxHealth = 100;
    public float tempHealth;
    public bool isDead;

    // the higher the number, more damage the player takes
    // the lower the number, less damage the player takes
    // (multiplication)
    [Tooltip("The damage the player takes is multiplied by this number.")]
    public float damageTakenMultiplier = 1f;
    public float defaultDamageTakenMultiplier { get; private set; } // properties don't show in inspector :P

    [Tooltip("If checked, the player will be invincible.")]
    public bool invincible;
    // Start is called before the first frame update
    void Start()
    {
        defaultDamageTakenMultiplier = damageTakenMultiplier;
        tempHealth = 100;
        InGameUI.SetHp(1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (tempHealth > maxHealth)
            tempHealth = maxHealth;
        else if (tempHealth <= 0)
        {
            tempHealth = 0;
            isDead = true;
        }
    }

    public void decreaseHealth(float num)
    {
        if (!invincible) {
            tempHealth -= num * damageTakenMultiplier;
            InGameUI.SetHp(tempHealth / maxHealth);
        }
    }

    public void increaseHealth(float num)
    {
        tempHealth += num;
        // Debug.Log(tempHealth);
        InGameUI.SetHp(tempHealth / maxHealth);
    }
}
