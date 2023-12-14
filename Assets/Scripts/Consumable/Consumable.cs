using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public Type ConsumableType = Type.None;

    public enum Type {
        None, Health, Screenwipe, Invincibility, OverpoweredBuff, LevelUp
    }

    static IEnumerator InvincibilityPayload() { // make the player not die
        PlayerHealth ph = Player.instance.GetComponent<PlayerHealth>();
        DamageFlash df = Player.instance.GetComponent<DamageFlash>();

        ph.invincible = true;
        df.HealthyColor = ConsumableManager.Instance.InvincibleColor;
        ConsumableManager.Instance.PlayerInvincible.Invoke();
        
        yield return new WaitForSeconds(ConsumableManager.Instance.InvincibilityDuration);
        
        df.HealthyColor = Color.white;
        ph.invincible = false;
        ConsumableManager.Instance.PlayerVulnerable.Invoke();
    }

    static IEnumerator OverpoweredBuffPayload() { // make the player strong
        var damageMult = ConsumableManager.Instance.DamageDealtMutliplier;
        var fireRateMult = ConsumableManager.Instance.FireRateMutliplier;
        var walkSpeedMult = ConsumableManager.Instance.WalkSpeedMutliplier;
        var damageTakenMult = ConsumableManager.Instance.DamageTakenMutliplier;
        var buffDuration = ConsumableManager.Instance.OverpoweredBuffDuration;
        
        // increase damage and fire rate
        ProjectileWeapon.staticStatModifiers.damage += damageMult;
        ProjectileWeapon.staticStatModifiers.fireRate += fireRateMult;

        // increase walk speed
        BuffableStat.Receipt moveSpeedReceipt = Player.instance.moveSpeedMultiplier.MultiplierBuff(walkSpeedMult);

        // decrease damage taken
        PlayerHealth ph = Player.instance.GetComponent<PlayerHealth>();
        ph.damageTakenMultiplier = damageTakenMult;

        ConsumableManager.Instance.PlayerOverpowered.Invoke();

        yield return new WaitForSeconds(buffDuration);
        
        ProjectileWeapon.staticStatModifiers.damage -= damageMult;
        ProjectileWeapon.staticStatModifiers.fireRate -= fireRateMult;

        // reset walk speed
        moveSpeedReceipt.Unbuff();

        // reset damage taken multiplier
        ph.damageTakenMultiplier = ph.defaultDamageTakenMultiplier;
        
        ConsumableManager.Instance.PlayerNotOverpowered.Invoke();
    }

    public static bool CanApply(Type consumableType) {
        if (consumableType == Type.OverpoweredBuff)
            return !ConsumableManager.Instance.OverpoweredBuffActive;
        if (consumableType == Type.Invincibility)
            return !ConsumableManager.Instance.InvincibilityActive;
        return true;
    }

    // should this be an instance method instead of a static method? lol
    public static void Apply(Type consumableType) {
        Debug.Log("Applying consumable " + consumableType);

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
