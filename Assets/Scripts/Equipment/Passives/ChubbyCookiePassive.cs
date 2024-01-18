using System;
using UnityEngine;

public class ChubbyCookiePassive : Passive {
    //increase in weapon size
    public float projectileSizeIncrease = 1.5f;
    float state = 0f;

    //the Player prefab
    private GameObject playerPrefab;

    public override (string description, Action onApply) GetLevelUps() {
        return ("Increase weapon size", applyProjectileSize);
    }

    public void applyProjectileSize() {
        state += projectileSizeIncrease;
        ProjectileWeapon.staticStatModifiers.size += projectileSizeIncrease;
    }

    public override void OnEquip() {
        applyProjectileSize();
    }

    public override void OnUnEquip() {     }
    protected override object FreezeRaw() { return state; }
    protected override void Thaw(object data) {
        state = (float)data;
        ProjectileWeapon.staticStatModifiers.size += state;
    }
}