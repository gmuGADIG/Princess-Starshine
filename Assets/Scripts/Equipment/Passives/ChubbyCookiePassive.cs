using System;
using UnityEngine;

public class ChubbyCookiePassive : Passive
{
    //increase in weapon size
    public float projectileSizeIncrease = 1.5f;

    //the Player prefab
    private GameObject playerPrefab;

    public override (string description, Action onApply) GetLevelUps() {
        return ("Increase weapon size", applyProjectileSize);
    }

    public void applyProjectileSize()
    {
        ProjectileWeapon.staticStatModifiers.ProjectileSize += projectileSizeIncrease;
    }

    public override void OnEquip() {
        applyProjectileSize();
    }

    public override void OnUnEquip() {     }
}
