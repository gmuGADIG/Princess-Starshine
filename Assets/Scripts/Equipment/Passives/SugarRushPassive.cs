using System;
using UnityEngine;

public class SugarRushPassive : Passive
{
    //Amount of damage dealt by weapon
    public float damageIncrease = 1.5f;

    //The Player prefab
    private GameObject playerPrefab;

    public SugraRushPassive()
    {
        this.type = EquipmentType.SugarRush;
    }
    
    public override (string description, Action onApply) GetLevelUps() 
    {
        return ("Greater weapon damage", applyDamage);
    }

    public void applyDamage() 
    {
        if (GameObject.FindGameObjectWithTag("Player") != null) 
        {
            playerPrefab = GameObject.FindGameObjectWithTag("Player");
            if (playerPrefab.GetComponentInChildren<EquipmentManager>() != null)
            {
                EquipmentManager playerEquipment = playerPrefab.GetComponentInChildren<EquipmentManager>();

                foreach (ProjectileWeapon weapon in playerEquipment.allWeapons)
                {
                    weapon.increaseDamage(damageIncrease);
                }
            }
            else 
            {
                Debug.LogError("Player is missing 'Player' tag");
            }
        }
        else 
        {
            Debug.LogError("Player is missing 'Player' tag");
        }
    }

    public override void OnEquip() {
        applyDamage();
    }

    public override void OnUnEquip() {      }

}
