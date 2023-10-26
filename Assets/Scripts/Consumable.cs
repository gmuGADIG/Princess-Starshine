using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    // TODO: put these somewhere design team can edit them
    public const float HealthConsumableHealthing = 100f;
    public const float InvincibilityDuration = 5f;

    [SerializeField]
    public Type ConsumableType = Type.None;

    public enum Type {
        None, Health, Screenwipe, Invincibility, OverpoweredBuff, LevelUp
    }

    IEnumerator InvincibilityPayload()
    {
        PlayerHealth ph = Player.instance.GetComponent<PlayerHealth>();

        yield return new WaitForSeconds(10);

        // now do something
    }

    public static void Apply(Type consumableType) {
        if (consumableType == Type.Health) {
            Player.instance.GetComponent<PlayerHealth>().increaseHealth(HealthConsumableHealthing);
        } else if (consumableType == Type.Screenwipe) {
            foreach (EnemyTemplate enemy in FindObjectsOfType<EnemyTemplate>()) {
                enemy.Die();
            }
        } else if (consumableType == Type.Invincibility) {

        } else if (consumableType == Type.OverpoweredBuff) {

        } else if (consumableType == Type.LevelUp) {

        } else {
            Debug.LogError("Unreachable in Consumable.Apply against variant " + consumableType);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
