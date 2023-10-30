using UnityEngine;
using UnityEngine.Events;

// Responsible for spawning consumables and holding the configuration for consumables
public class ConsumableManager : MonoBehaviour
{
    public static ConsumableManager Instance { get; private set; }

    public float ConsumableSpawnChance = 0.01f;
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

    void Start() {
        GameObject[] consumablePrefabs = Resources.LoadAll<GameObject>("Consumables");

        // chance to spawn a consumable every time an enemy spawns
        EnemySpawner.SpawningEnemy.AddListener(() => { 
            if (Random.value < ConsumableSpawnChance) {
                // pick a random point in the camera bounds
                Rect cameraBounds = TeaTime.cameraBoundingBox();
                Vector3 pos = new Vector3(
                    cameraBounds.x + Random.Range(0f, cameraBounds.width),
                    cameraBounds.y + Random.Range(0f, cameraBounds.height)
                );

                // yes, Random.Range is max exclusive for ints and max inclusive for floats
                GameObject prefab = consumablePrefabs[Random.Range(0, consumablePrefabs.Length)];
                Instantiate(prefab, pos, Quaternion.identity);
            }
        });
    }
}
