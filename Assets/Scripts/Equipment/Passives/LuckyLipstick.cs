using System;
using UnityEngine;

public class LuckyLipstick : Passive {
    public float InitialCollisionRadiusMultiplier = 3f;
    public float InitialSpawnChanceMultiplier = 3f;

    public float CollisionRadiusScalar = 2f;
    public float SpawnChangeScalar = 1.25f;

    private BuffableStat.Receipt collisionRadiusReceipt;
    private BuffableStat.Receipt spawnChanceReceipt;

    public LuckyLipstick() {
        type = EquipmentType.LuckyLipstick;
    }

    public override (string description, Action onApply) GetLevelUps() {
        return (
            description: "Increase consumable spawn chance and consumable pickup radius",
            onApply: () => {
                Debug.Log($"collisionRadiusReceipt != null: {collisionRadiusReceipt}");
                Debug.Log($"collisionRadiusReceipt.Stat != null: {collisionRadiusReceipt.Stat}");

                collisionRadiusReceipt.Rebuff(collisionRadiusReceipt.Value * CollisionRadiusScalar);
                spawnChanceReceipt.Rebuff(spawnChanceReceipt.Value * CollisionRadiusScalar);
            }
        );
    }

    public override void OnEquip() {
        collisionRadiusReceipt = ConsumableManager.Instance.ConsumableCollisionRadius.MultiplierBuff(InitialCollisionRadiusMultiplier);
        spawnChanceReceipt = ConsumableManager.Instance.ConsumableSpawnChance.MultiplierBuff(InitialSpawnChanceMultiplier);
    }

    public override void OnUnEquip() {
        collisionRadiusReceipt.Unbuff();
        spawnChanceReceipt.Unbuff();
    }
}