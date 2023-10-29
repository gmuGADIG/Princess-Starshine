using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public Type ConsumableType { get; private set; } 

    public enum Type {
        None, Health, Screenwipe, Invincibility, OverpoweredBuff, LevelUp
    }

    static IEnumerator InvincibilityPayload() { // make the player not die
        PlayerHealth ph = Player.instance.GetComponent<PlayerHealth>();

        ph.invincible = true;
        ConsumableManager.Instance.PlayerInvincible.Invoke();
        
        yield return new WaitForSeconds(ConsumableManager.Instance.InvincibilityDuration);
        
        ph.invincible = false;
        ConsumableManager.Instance.PlayerVulnerable.Invoke();
    }

    static IEnumerator OverpoweredBuffPayload() { // make the player strong
        // increase damage dealt
        // WARN: this does not work for weapons which aren't projectile weapons!
        ProjectileWeapon.damageMultiplier = ConsumableManager.Instance.DamageDealtMutliplier;

        // increase fire rate
        // WARN: this does not work for weapons which aren't projectile weapons!
        ProjectileWeapon.fireRateMultiplier = ConsumableManager.Instance.FireRateMutliplier;

        // increase walk speed
        Player.instance.moveSpeedMultiplier = ConsumableManager.Instance.WalkSpeedMutliplier;

        // decrease damage taken
        PlayerHealth ph = Player.instance.GetComponent<PlayerHealth>();
        ph.damageTakenMultiplier = ConsumableManager.Instance.DamageTakenMutliplier;

        ConsumableManager.Instance.PlayerOverpowered.Invoke();

        yield return new WaitForSeconds(ConsumableManager.Instance.OverpoweredBuffDuration);
        
        ProjectileWeapon.fireRateMultiplier = 1f;

        ProjectileWeapon.damageMultiplier = 1f;

        // reset walk speed
        Player.instance.moveSpeedMultiplier = ConsumableManager.Instance.WalkSpeedMutliplier;

        // reset damage taken multiplier
        ph.damageTakenMultiplier = ph.defaultDamageTakenMultiplier;
        
        ConsumableManager.Instance.PlayerNotOverpowered.Invoke();
    }

    public static void Apply(Type consumableType) {
        if (consumableType == Type.Health) { // heal the player
            Player.instance.GetComponent<PlayerHealth>().increaseHealth(ConsumableManager.Instance.HealthConsumableHealing);
        } else if (consumableType == Type.Screenwipe) { // kill everything
            foreach (EnemyTemplate enemy in FindObjectsOfType<EnemyTemplate>()) {
                enemy.Die();
            }
        } else if (consumableType == Type.Invincibility) { // make the player not die
            // attach the coroutine to the player, we cant start coroutines, we're not a monobehaviour :(
            Player.instance.StartCoroutine(InvincibilityPayload());
        } else if (consumableType == Type.OverpoweredBuff) { // make the player strong
            // attach the coroutine to the player, we cant start coroutines, we're not a monobehaviour :(
            Player.instance.StartCoroutine(OverpoweredBuffPayload());
        } else if (consumableType == Type.LevelUp) { // level up the player
            Player.instance.LevelUp();
        } else { // panic
            Debug.LogError("Unreachable in Consumable.Apply against variant " + consumableType);
        }
    }
    void Start()
    {
        if (ConsumableType == Type.None) {
            Debug.LogError("Consumable has type none!");
        }
    }
}
