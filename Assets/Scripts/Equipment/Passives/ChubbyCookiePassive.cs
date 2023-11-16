using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChubbyCookiePassive : Passive
{
    //Amount of projectiles fired by weapon
    public float attackSizeIncrease = 1.5f;
    
    //The Player prefab
    private GameObject playerPrefab;

    public ChubbyCookiePassive() 
    {
        this.type = EquipmentType.ChubbyCookie;
    }

    public override (string description, Action onApply) GetLevelUps() {
        return ("Greater weapon fire rate", applyAttackSize);
    }

    public void applyAttackSize()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null) 
        {
            playerPrefab = GameObject.FindGameObjectWithTag("Player");
            if (playerPrefab.GetComponentInChildren<EquipmentManager>() != null)
            {
                EquipmentManager playerEquipment = playerPrefab.GetComponentInChildren<EquipmentManager>();

                foreach (ProjectileWeapon weapon in playerEquipment.allWeapons)
                {
                    weapon.increaseAttackSize(attackSizeIncrease);
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
        applyAttackSize();
    }

    public override void OnUnEquip() {     }
}
