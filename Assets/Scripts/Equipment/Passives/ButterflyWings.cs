using System;
using UnityEngine;

public class ButterflyWings : Passive {
    // how much does this passive make the player move faster?
    public float walkSpeedMultiplier = 1.5f;
    private BuffableStat.Receipt receipt;
    
    // how much more should this passive move the player after each upgrade
    public float scaleMultiplier = 1.5f;
    
    public override (string description, Action onApply) GetLevelUps() {
        return (
            description: "Increases your move speed",
            onApply: () => {
                receipt = receipt.Rebuff(receipt.Value * scaleMultiplier);
            }
        );
    }

    public override void OnEquip() {
        Debug.Log("Equipping Butterfly Wings");
        receipt = Player.instance.moveSpeedMultiplier.MultiplierBuff(walkSpeedMultiplier);
    }

    public override void OnUnEquip() {
        receipt.Unbuff();
    }
}