using System;
using UnityEngine;

public class SugarRushPassive : Passive
{
    //Amount of damage dealt by weapon
    public float damageIncrease = 1.5f;

    public override (string description, Action onApply) GetLevelUps() 
    {
        return ("Greater weapon damage", applyDamage);
    }

    public void applyDamage() 
    {
        ProjectileWeapon.staticStatModifiers.damage += damageIncrease;
    }

    public override void OnEquip() {
        applyDamage();
    }

    public override void OnUnEquip() {      }

}
