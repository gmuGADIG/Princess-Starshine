using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnySqueePassive : Passive
{   

    //Amount of projectiles fired by weapon
    public float fireRateIncrease = 1.5f;
    
    //The Player prefab
    private gameObject playerPrefab;
    public SunnySqueePassive() 
    {
        this.type = EquipmentType.SunnySquee;
    }

    public void Update() 
    {

    }

    public void applyFireRate()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null) 
        {
            playerPrefab = GameObject.FindGameObjectWithTag("Player");
            if (playerPrefab.GetComponentInChildren<EquipmentManager>() != null)
            {
                EquipmentManager playerEquipment = playerPrefab.GetComponentInChildren<EquipmentManager>();

                foreach (ProjectileWeapon weapon in playerEquipment.allWeapons)
                {
                    weapon.increaseFireRate(fireRateIncrease);
                }
            }
            else 
            {
                Debug.LogError("Player is missing 'Player' tag")
            }
        }
        else 
        {
            Debug.LogError("Player is missing 'Player' tag")
        }
    }

    public override void OnEquip() {
        applyFireRate();
    }

    public override void OnUnEquip() { 
        
    }

    public override void ProcessOther(Equipment other) { 
        //pass
    }

    public override void ProcessOtherRemoval(Equipment other) { 
        //pass
    }

    public override (string description, Action onApply) GetLevelUps() {
        return ("Greater weapon fire rate", applyFireRate)
    }
}
