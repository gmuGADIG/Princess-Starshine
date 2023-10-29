using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Responsible for spawning consumables and holding the configuration for consumables
public class ConsumableManager : MonoBehaviour
{
    public static ConsumableManager Instance { get; private set; }

    public float HealthConsumableHealing = 100f;
    public float InvincibilityDuration = 5f;
    [Header("Overpowered Buff")]
    public float OverpoweredBuffDuration = 5f;
    public float DamageDealtMutliplier = 5f;
    public float WalkSpeedMutliplier = 5f;
    public float FireRateMutliplier = 5f;
    public float DamageTakenMutliplier = 0.2f;

    public UnityEvent PlayerInvincible = new UnityEvent();
    public UnityEvent PlayerVulnerable = new UnityEvent();

    public UnityEvent PlayerOverpowered = new UnityEvent();
    public UnityEvent PlayerNotOverpowered = new UnityEvent();

    void Awake() {
        Instance = this;
    }
}
