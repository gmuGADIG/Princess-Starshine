using System;
using Unity.VisualScripting;
using UnityEngine;

public class SunnySqueePassive : Passive
{   
    //Amount of projectiles fired by weapon
    public float fireRateIncrease = 1.5f;
    float state = 0;
    
    //The Player prefab
    private GameObject playerPrefab;

    public override (string description, Action onApply) GetLevelUps() {
        return ("Greater weapon fire rate", applyFireRate);
    }

    public void applyFireRate()
    {
        state += fireRateIncrease;
        ProjectileWeapon.staticStatModifiers.fireRate += fireRateIncrease;
    }

    public override void OnEquip() {
        applyFireRate();
    }

    public override void OnUnEquip() {     }

    public override void Thaw(Equipment equipment)
    {
        var trueEquipment = (SunnySqueePassive)equipment;

        ProjectileWeapon.staticStatModifiers.fireRate -= state;
        state = trueEquipment.state;
        ProjectileWeapon.staticStatModifiers.fireRate += state;
    }
}
