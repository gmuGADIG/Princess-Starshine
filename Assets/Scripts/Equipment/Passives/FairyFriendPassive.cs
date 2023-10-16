using System;
using UnityEngine;

// TODO: complete fairy friend passive

public class FairyFriendPassive : Passive
{
    public FairyFriendPassive()
    {
        this.type = EquipmentType.FairyFriend;
    }
    
    public override void OnEquip()
    {
        Debug.LogWarning("Fairy Friend not implemented yet!");
    }

    public override void OnUnEquip() { }

    public override (string description, Action onApply) GetLevelUps()
    {
        return ("Greater health regen", () => Debug.Log("Not implemented yet!"));
    }
}
