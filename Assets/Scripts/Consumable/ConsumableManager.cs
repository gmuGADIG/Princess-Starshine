using System;
using UnityEngine;
using UnityEngine.Events;

// Responsible for spawning consumables and holding the configuration for consumables
public class ConsumableManager : MonoBehaviour
{
    public static ConsumableManager Instance { get; private set; }

    [SerializeField] private float initialConsumableSpawnChance = 0.01f;
    public float HealthConsumableHealing = 100f;
    public float InvincibilityDuration = 5f;
    [SerializeField] private float initialConsumableCollisionRadius = 0.5f;

    public Color InvincibleColor = Color.yellow;

    [Header("Overpowered Buff")]
    public float OverpoweredBuffDuration = 5f;
    public float DamageDealtMutliplier = 5f;
    public float WalkSpeedMutliplier = 5f;
    public float FireRateMutliplier = 5f;
    public float DamageTakenMutliplier = 0.2f;

    public bool OverpoweredBuffActive { get; private set; } = false;
    public bool InvincibilityActive { get; private set; } = false;

    public UnityEvent PlayerInvincible = new UnityEvent();
    public UnityEvent PlayerVulnerable = new UnityEvent();

    public UnityEvent PlayerOverpowered = new UnityEvent();
    public UnityEvent PlayerNotOverpowered = new UnityEvent();

    [HideInInspector] public BuffableStat ConsumableCollisionRadius { get; private set; }
    [HideInInspector] public BuffableStat ConsumableSpawnChance { get; private set; }
    void Awake() {
        Instance = this;

        ConsumableCollisionRadius = new BuffableStat(initialConsumableCollisionRadius);
        ConsumableSpawnChance = new BuffableStat(initialConsumableSpawnChance);
    }

    void Start() {
        GameObject[] consumablePrefabs = Resources.LoadAll<GameObject>("Consumables");

        ConsumableCollisionRadius.ValueUpdated.AddListener((radius) => { // Update consumable collision radius when the value changes
            foreach (Consumable c in FindObjectsOfType<Consumable>())
                c.GetComponent<CircleCollider2D>().radius = radius;
        });

        // chance to spawn a consumable every time an enemy spawns
        EnemySpawner.SpawningEnemy.AddListener(() => { 
            //Debug.Log($"ConsumableSpawnChance.Value: {ConsumableSpawnChance.Value}");
            if (UnityEngine.Random.value < ConsumableSpawnChance.Value) {
                // pick a random point in the camera bounds
                Rect cameraBounds = TeaTime.cameraBoundingBox();
                Vector3 pos = new Vector3(
                    cameraBounds.x + UnityEngine.Random.Range(0f, cameraBounds.width),
                    cameraBounds.y + UnityEngine.Random.Range(0f, cameraBounds.height)
                );

                // yes, Random.Range is max exclusive for ints and max inclusive for floats
                GameObject prefab = consumablePrefabs[UnityEngine.Random.Range(0, consumablePrefabs.Length)];
                GameObject consumable = Instantiate(prefab, pos, Quaternion.identity);
                consumable.GetComponent<CircleCollider2D>().radius = ConsumableCollisionRadius.Value;
            }
        });

        PlayerInvincible.AddListener(() => { InvincibilityActive = true; });
        PlayerVulnerable.AddListener(() => { InvincibilityActive = false; });

        PlayerOverpowered.AddListener(() => { OverpoweredBuffActive = true; });
        PlayerNotOverpowered.AddListener(() => { OverpoweredBuffActive = false; });
    }
}
