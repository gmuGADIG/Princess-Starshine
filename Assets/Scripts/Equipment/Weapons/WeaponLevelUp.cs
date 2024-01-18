
using System;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public enum WeaponLevelUpType {
    // float modifiers (1 = +100%)
    Damage, Knockback,
    FireRate, 
    ProjectileSize, ProjectileSpeed,
    
    // int modifiers (1 = +1)
    MaxProjectiles, ProjectilesPerShot, Pierce,
   
    // float modifiers (1 = +100%)
    DotRate,
}

[Serializable]
public class WeaponLevelUp {
    public WeaponLevelUpType type;
    public float amount;

    public override string ToString() {
        var propertyName = Regex.Replace(type.ToString(), "(\\B[A-Z])", " $1"); // add spaces, e.g. "FireRate" -> "Fire Rate"
        if (type is WeaponLevelUpType.Pierce or WeaponLevelUpType.ProjectilesPerShot or WeaponLevelUpType.MaxProjectiles) // int modifiers {
            return $"+{amount} {propertyName}";
        }
        else // float modifiers (show as percent) {
            // e.g. 1.3 -> +130%
            return $"+{amount:P0} {propertyName}";
        }
    }
}
