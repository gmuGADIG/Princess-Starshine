using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMirrorPassive : Passive
{
    //Orbit rate around player
    public float orbitRate = 1.5f;

    //Orbital size
    public float size = 1.5f;

    //The Player Prefab
    private GameObject playerPrefab;

    public MagicMirrorPassive() 
    {
        this.type = EquipmentType.MagicMirror;
    }
    
    public override (string description, Action onApply) GetLevelUps()
    {
        return ("Something", onApply);
    }

    public override void OnEquip() {
        
    }

    public override void OnUnEquip() {      }

}
