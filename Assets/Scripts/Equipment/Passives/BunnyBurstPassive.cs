using System;
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

    public override (string description, Action onApply) GetLevelUps()
    {
        return ("Greater projectile speed boost", () => Debug.Log("Not implemented yet!"));
    }

}
