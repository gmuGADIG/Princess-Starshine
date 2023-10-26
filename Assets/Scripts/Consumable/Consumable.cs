using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public Type ConsumableType { get; private set; } 

    public enum Type {
        None, Health, Screenwipe, Invincibility, OverpoweredBuff, LevelUp
    }

    static IEnumerator InvincibilityPayload() {
        PlayerHealth ph = Player.instance.GetComponent<PlayerHealth>();

        ph.invincible = true;
        ConsumableManager.Instance.PlayerInvincible.Invoke();
        
        yield return new WaitForSeconds(ConsumableManager.Instance.InvincibilityDuration);
        
        ph.invincible = false;
        ConsumableManager.Instance.PlayerVulnerable.Invoke();
    }

    public static void Apply(Type consumableType) {
        if (consumableType == Type.Health) {
            Player.instance.GetComponent<PlayerHealth>().increaseHealth(ConsumableManager.Instance.HealthConsumableHealing);
        } else if (consumableType == Type.Screenwipe) {
            foreach (EnemyTemplate enemy in FindObjectsOfType<EnemyTemplate>()) {
                enemy.Die();
            }
        } else if (consumableType == Type.Invincibility) {
            Player.instance.StartCoroutine(InvincibilityPayload());
        } else if (consumableType == Type.OverpoweredBuff) {
            // TODO: "increase all of the stats"
        } else if (consumableType == Type.LevelUp) {
            Player.instance.LevelUp();
        } else {
            Debug.LogError("Unreachable in Consumable.Apply against variant " + consumableType);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ConsumableType == Type.None) {
            Debug.LogError("Consumable has type none!");
        }
    }
}
