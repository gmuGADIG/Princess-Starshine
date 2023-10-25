using System;


[Serializable]
public abstract class Weapon : Equipment
{
    /** The LevelUps that this weapon is capable of receiving on an upgrade. */
    public WeaponLevelUp[] levelUpOptions;
    
    public abstract void ApplyLevelUp(WeaponLevelUp levelUp);
}
