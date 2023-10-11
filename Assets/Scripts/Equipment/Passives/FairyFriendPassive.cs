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

    public override string GetLevelUpDescription()
    {
        return "Greater health regen";
    }

    public override void ApplyLevelUp()
    {
        Debug.LogWarning("Fairy Friend not implemented yet!");
    }
}
