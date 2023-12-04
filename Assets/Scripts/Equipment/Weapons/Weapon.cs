using System;

// NOTE: if you are opting to extend this instead of using an instance ProjectileWeapon
// please recitfy the code in Consumable.OverpoweredBuffPayload

[Serializable]
public abstract class Weapon : Equipment
{
    /** The LevelUps that this weapon is capable of receiving on an upgrade. */
    public WeaponLevelUp[] levelUpOptions;
}
