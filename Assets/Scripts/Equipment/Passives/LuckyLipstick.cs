using System;
using UnityEngine;

public class LuckyLipstick : Passive {
    [Tooltip("How much the pick up range increases when the passive is first unlocked.")]
    public float InitialPickUpRangeMultiplier = 2f;
    [Tooltip("How much the consumable spawn chance increases when the passive is first unlocked.")]
    public float InitialSpawnChanceMultiplier = 2f;

    [Tooltip("How much the pick up range increases every upgrade.")]
    public float PickUpRangeScalar = 2f;
    [Tooltip("How much the consumable spawn chance increases every upgrade.")]
    public float SpawnChanceScalar = 1f;

    private BuffableStat.Receipt collisionRadiusReceipt;
    private BuffableStat.Receipt spawnChanceReceipt;
    private BuffableStat.Receipt xpRadiusReceipt;

    public override (string description, Action onApply) GetLevelUps() {
        return (
            description: "Increase consumable spawn chance and consumable pickup radius",
            onApply: () => {
                Debug.Log($"collisionRadiusReceipt != null: {collisionRadiusReceipt}");
                Debug.Log($"collisionRadiusReceipt.Stat != null: {collisionRadiusReceipt.Stat}");

                collisionRadiusReceipt = collisionRadiusReceipt.Rebuff(collisionRadiusReceipt.Value * (PickUpRangeScalar + 1));
                spawnChanceReceipt = spawnChanceReceipt.Rebuff(spawnChanceReceipt.Value * (SpawnChanceScalar + 1));
                xpRadiusReceipt = xpRadiusReceipt.Rebuff(xpRadiusReceipt.Value * (PickUpRangeScalar + 1));
            }
        );
    }

    public override void OnEquip() {
        collisionRadiusReceipt = ConsumableManager.Instance.ConsumableCollisionRadius.MultiplierBuff(InitialPickUpRangeMultiplier + 1);
        spawnChanceReceipt = ConsumableManager.Instance.ConsumableSpawnChance.MultiplierBuff(InitialSpawnChanceMultiplier + 1);
        xpRadiusReceipt = ConsumableManager.Instance.XpCollisionRadius.MultiplierBuff(InitialPickUpRangeMultiplier + 1);
    }

    public override void OnUnEquip() {
        collisionRadiusReceipt.Unbuff();
        spawnChanceReceipt.Unbuff();
    }
    protected override object FreezeRaw() { 
        return (collisionRadiusReceipt?.Value ?? 0f, spawnChanceReceipt?.Value ?? 0f); 
    }
    protected override void Thaw(object _data) {
        var (radiusMultiplier, spawnChance) = ((float, float))_data;
        collisionRadiusReceipt = ConsumableManager.Instance.ConsumableCollisionRadius.MultiplierBuff(radiusMultiplier);
        spawnChanceReceipt = ConsumableManager.Instance.ConsumableSpawnChance.MultiplierBuff(spawnChance);
    }
}