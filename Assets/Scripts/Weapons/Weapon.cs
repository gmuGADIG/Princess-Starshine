using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    //Declare it in the projectile prefab, too much effort to transfer over the data from the weapon to the projectile
    //[SerializeField]
    //protected int damage;

    //
    [SerializeField]
    protected float cooldownTimeSeconds;

    protected float currentCooldownTime;

    //The number of enemies the projectile needs to hit before dissapearing. Set to 0 to ignore enemies.
    //[SerializeField]
    //protected bool doesDOT;

    public abstract void Update();

}