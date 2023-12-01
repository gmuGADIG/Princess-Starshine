using System;
using UnityEngine;

// TODO: complete bunny burst passive

public class BunnyBurstPassive : Passive
{
    public float speedIncrease = 2f;

    public override void OnEquip()
    {
        applySpeed();
    }

    public override void OnUnEquip() { }

    public override (string description, Action onApply) GetLevelUps()
    {
        return ("Greater projectile speed boost", applySpeed);
    }

    void applySpeed()
    {
        ProjectileWeapon.staticStatModifiers.projectileSpeed += speedIncrease;
    }

}
