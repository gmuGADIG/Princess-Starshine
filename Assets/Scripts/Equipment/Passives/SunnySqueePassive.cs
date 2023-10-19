using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnySqueePassive : Passive
{
    public SunnySqueePassive()
    {
        this.type = EquipmentType.SunnySqueePassive;
    }

    public override void OnEquip()
    {
        Debug.LogWarning("Sunny Squee not implemented yet!");
    }

    public override void OnUnEquip() { }

    public override string GetLevelUpDescription()
    {
        return "Higher fire rate";
    }

    public override void ApplyLevelUp()
    {
        Debug.LogWarning("Sunny Squee not implemented yet!");
    }
}
