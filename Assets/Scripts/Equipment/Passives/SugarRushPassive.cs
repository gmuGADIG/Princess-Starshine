using System;
using UnityEngine;

public class SugarRushPassive : Passive {
    //Amount of damage dealt by weapon
    public float damageIncrease = 1.5f;
    float state = 0f;

    public override (string description, Action onApply) GetLevelUps()  {
        return ("Greater weapon damage", applyDamage);
    }

    public void applyDamage()  {
        state += damageIncrease;
        ProjectileWeapon.staticStatModifiers.damage += damageIncrease;
    }

    public override void OnEquip() {
        applyDamage();
    }

    public override void OnUnEquip() {      }

    protected override object FreezeRaw() { return state; }
    protected override void Thaw(object data) {
        state = (float)data;
        ProjectileWeapon.staticStatModifiers.damage += state;
    }
}
