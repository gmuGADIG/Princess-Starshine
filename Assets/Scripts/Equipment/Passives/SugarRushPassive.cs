using System;
using UnityEngine;

public class SugarRushPassive : Passive
{
    //Amount of damage dealt by weapon
    public float damageIncrease = 1.5f;
    float state = 0f;

    public override (string description, Action onApply) GetLevelUps() 
    {
        return ("Greater weapon damage", applyDamage);
    }

    public void applyDamage() 
    {
        state += damageIncrease;
        ProjectileWeapon.staticStatModifiers.damage += damageIncrease;
    }

    public override void OnEquip() {
        applyDamage();
    }

    public override void OnUnEquip() {      }


    public override void Thaw(Equipment equipment)
    {
        var trueEquipment = (SugarRushPassive)equipment;

        ProjectileWeapon.staticStatModifiers.damage -= state;
        state = trueEquipment.state;
        ProjectileWeapon.staticStatModifiers.damage += state;
    }
}
