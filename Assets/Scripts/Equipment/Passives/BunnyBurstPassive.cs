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

    protected override object FreezeRaw() { return state; }
    protected override void Thaw(object data) {
        state = (float)data;
        ProjectileWeapon.staticStatModifiers.projectileSpeed += state;
    }
}
