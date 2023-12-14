using System;
using UnityEngine;

public class ChubbyCookiePassive : Passive
{
    //increase in weapon size
    public float projectileSizeIncrease = 1.5f;
    float state = 0f;

    //the Player prefab
    private GameObject playerPrefab;

    public override (string description, Action onApply) GetLevelUps() {
        return ("Increase weapon size", applyProjectileSize);
    }

    public void applyProjectileSize()
    {
        state += projectileSizeIncrease;
        ProjectileWeapon.staticStatModifiers.size += projectileSizeIncrease;
    }

    public override void OnEquip() {
        applyProjectileSize();
    }

    public override void OnUnEquip() {     }

    public override void Thaw(Equipment equipment)
    {
        var trueEquipment = (ChubbyCookiePassive)equipment;

        ProjectileWeapon.staticStatModifiers.size -= state;
        state = trueEquipment.state;
        ProjectileWeapon.staticStatModifiers.size += state;
    }
}