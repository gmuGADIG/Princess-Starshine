using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnySqueePassive : Passive
{
    //Amount of fire rate increased per use
    public float fireRate = 2.5f;

    //Amount fire rate increased per level up
    public float fireRateUpgrade = 1.5f;

    //The Player prefab
    private GameObject playerPrefab;

    public SunnySqueePassive()
    {
        this.type = EquipmentType.SunnySquee;
    }

    public override void OnEquip()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)

        {
            playerPrefab = GameObject.FindGameObjectWithTag("Player");
            if (playerPrefab.GetComponent<EquipmentType>() != null)

            {
                //Increase fire rate
                foreach (ProjectileWeapon weapon in playerEquipment.allWeapons)
                {
                    weapon.increaseFireRate(fireRateIncrease);
                }
            }
        }
    }

    public override void OnUnEquip() { }

    public override string GetLevelUpDescription()
    {
        return "Greater weapon fire rate";
    }

    public override void ApplyLevelUp()
    {
        fireRate += fireRateUpgrade;
    }
}
