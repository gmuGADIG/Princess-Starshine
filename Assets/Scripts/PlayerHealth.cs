using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Action PlayerDied;

    [Tooltip("The player's max health.")]
    public float maxHealth = 100;
    public float tempHealth { get; private set; }
    public bool isDead { get; private set; }

    // the higher the number, more damage the player takes
    // the lower the number, less damage the player takes
    // (multiplication)
    [Tooltip("The damage the player takes is multiplied by this number.")]
    public float damageTakenMultiplier = 1f;
    public float defaultDamageTakenMultiplier { get; private set; } // properties don't show in inspector :P

    [Tooltip("If checked, the player will be invincible.")]
    public bool invincible;
    // Start is called before the first frame update
    
    [Tooltip("The amount the player heals per second.")]
    [SerializeField] float passiveHealAmount;

    void Start()
    {
        defaultDamageTakenMultiplier = damageTakenMultiplier;
        tempHealth = maxHealth;
        InGameUI.SetHp(1f);

        invincible = invincible && Application.isEditor;
    }

    void Update() {
        increaseHealth(passiveHealAmount * Time.deltaTime);
    }

    public void decreaseHealth(float num)
    {
        if (!invincible && !BossSceneManager.InPostDialogue) {
            SoundManager.Instance.PlaySoundGlobal(Player.instance.takeDamageSound);

            tempHealth -= num * damageTakenMultiplier;
            InGameUI.SetHp(tempHealth / maxHealth);
            GetComponent<DamageFlash>().Damage();

            if (tempHealth <= 0 && !isDead) {
                tempHealth = 0;
                isDead = true;

                PlayerDied?.Invoke();
            }
        }

    }

    public void increaseHealth(float num)
    {
        tempHealth += num;

        if (tempHealth > maxHealth) {
            tempHealth = maxHealth;
        }

        // Debug.Log(tempHealth);
        InGameUI.SetHp(tempHealth / maxHealth);
    }
}
