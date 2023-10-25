
using System;
using UnityEngine;

[Serializable]
public enum WeaponLevelUpType
{
    // multiplicative modifiers (e.g. times 1.5 damage); real number increments 
    Damage, KnockBack,
    FireRate,
    AoESize,
    ProjectileSize, ProjectileSpeed,
    
    // additive modifiers (e.g. +1 pierce); integer increments
    MaxProjectiles, ProjectileCount, Pierce,
}

[Serializable]
public class WeaponLevelUp
{
    public WeaponLevelUpType type;
    public float amount;

    public override string ToString()
    {
        if (type == WeaponLevelUpType.Pierce)
        {
            return $"+{amount} Pierces";
        }
        else
        {
            // e.g. 1.3 -> +30%
            var percentString = "+" + (amount - 1).ToString("P0");
            return $"{percentString} {type}";
        }
        // TODO: nicer output
    }
}