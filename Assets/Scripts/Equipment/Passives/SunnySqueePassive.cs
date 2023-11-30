using System;
using UnityEngine;

public class SunnySqueePassive : Passive
{   

    //Amount of projectiles fired by weapon
    public float fireRateIncrease = 1.5f;
    
    //The Player prefab
    private GameObject playerPrefab;

    public override (string description, Action onApply) GetLevelUps() {
        return ("Greater weapon fire rate", applyFireRate);
    }

    public void applyFireRate()
    {
        ProjectileWeapon.staticStatModifiers.fireRate += fireRateIncrease;
    }

    public override void OnEquip() {
        applyFireRate();
    }

    public override void OnUnEquip() {     }

}
