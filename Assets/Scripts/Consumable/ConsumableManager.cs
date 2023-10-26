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
    public float DamageModifier = 5f;
    public float WalkSpeedModifer = 5f;
    public float FireRateModifier = 5f;
    public float DamageTakenModifer = 0.2f;

    public UnityEvent PlayerInvincible { get; private set; }
    public UnityEvent PlayerVulnerable { get; private set; }

    void Awake() {
        Instance = this;
    }
}
