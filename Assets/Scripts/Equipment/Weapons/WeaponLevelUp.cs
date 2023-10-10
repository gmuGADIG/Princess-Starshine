
using System;
using UnityEngine;

[Serializable]
public enum WeaponLevelUpType
{
    Damage, KnockBack, Pierce,
    FireRate,
    AoESize,
    ProjectileCount, ProjectileSize, ProjectileSpeed 
}

[Serializable]
public class WeaponLevelUp
{
    public WeaponLevelUpType type;
    public float amount;

    public override string ToString()
    {
        return $"{amount} {type}"; // TODO: nicer output. include if it's additive / multiplicative (need to decide that first!)
    }
}