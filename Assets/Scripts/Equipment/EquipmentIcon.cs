using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Holds everything that's displayed when choosing a weapon or passive on the level-up screen.
 * Each EquipmentType should have one and only one EquipmentIcon set in the EquipmentManager.
 */
[Serializable]
public class EquipmentIcon
{
    public EquipmentType type;
    public string name;
    public string description;
    public Texture icon;
}
