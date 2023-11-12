using System;
using UnityEngine;

// TODO: complete bunny burst passive

public class BunnyBurstPassive : Passive
{
    public float speedIncrease = 2f;

    private GameObject playerPrefab;

    public BunnyBurstPassive()
    {
        this.type = EquipmentType.BunnyBurst;
    }

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
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerPrefab = GameObject.FindGameObjectWithTag("Player");
            if (playerPrefab.GetComponentInChildren<EquipmentManager>() != null)
            {
                EquipmentManager playerEquipment = playerPrefab.GetComponentInChildren<EquipmentManager>();

                //foreach (ProjectileWeapon weapon in playerEquipment.allWeapons)
                //{
                    //weapon.increaseProjectileSpeed(speedIncrease);
                //}
            }
            else
            {
                Debug.LogError("Player is missing PlayerHealth component");
            }
        }
        else
        {
            Debug.LogError("Player is missing 'Player' tag");
        }
    }

}
