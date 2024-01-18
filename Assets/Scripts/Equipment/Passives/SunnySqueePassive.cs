using System;
using Unity.VisualScripting;
using UnityEngine;

public class SunnySqueePassive : Passive {   
    //Amount of projectiles fired by weapon
    public float fireRateIncrease = 1.5f;
    float state = 0;
    
    //The Player prefab
    private GameObject playerPrefab;

    public override (string description, Action onApply) GetLevelUps() {
        return ("Greater weapon fire rate", applyFireRate);
    }

    public void applyFireRate() {
        state += fireRateIncrease;
        ProjectileWeapon.staticStatModifiers.fireRate += fireRateIncrease;
    }

    public override void OnEquip() {
        applyFireRate();
    }

    public override void OnUnEquip() {     }

    protected override object FreezeRaw() { return state; }
    protected override void Thaw(object data) {
        state = (float)data;
        ProjectileWeapon.staticStatModifiers.fireRate += state;
    }
}
