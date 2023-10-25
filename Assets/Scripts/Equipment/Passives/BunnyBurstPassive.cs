using UnityEngine;

// TODO: complete bunny burst passive

public class BunnyBurstPassive : Passive
{
    public BunnyBurstPassive()
    {
        this.type = EquipmentType.BunnyBurst;
    }
    
    public override void OnEquip()
    {
        Debug.LogWarning("Bunny Burst not implemented yet!");
    }

    public override void OnUnEquip() { }

    public override string GetLevelUpDescription()
    {
        return "Greater projectile speed boost";
    }

    public override void ApplyLevelUp()
    {
        Debug.LogWarning("Bunny Burst not implemented yet!");
    }
}
