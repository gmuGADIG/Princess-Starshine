using System;
using UnityEngine;

// TODO: complete bunny burst passive

public class BunnyBurstPassive : Passive
{
    public float speedIncrease = 2f;
    float state = 0f;

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
        state += speedIncrease;
        ProjectileWeapon.staticStatModifiers.projectileSpeed += speedIncrease;
    }


    public override void Thaw(Equipment equipment)
    {
        var trueEquipment = (BunnyBurstPassive)equipment;

        ProjectileWeapon.staticStatModifiers.projectileSpeed -= state;
        state = trueEquipment.state;
        ProjectileWeapon.staticStatModifiers.projectileSpeed += state;
    }
}
