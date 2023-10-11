using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/**
 * Singleton class for managing all equipment (weapons and passives).
 * Responsible for selecting equipment, holding what equipment the player currently has, and calling the Equipment's functions.
 */
public class EquipmentManager : MonoBehaviour
{
    // constants
    const int MAX_WEAPONS = 7;
    const int MAX_PASSIVES = 7;
    const int MAX_EQUIPMENT_LEVELS = 7;
    
    // in inspector
    public EquipmentIcon[] icons;
    
    // general
    public static EquipmentManager instance;
    [SerializeField] ProjectileWeapon[] allWeapons;
    public List<Equipment> allEquipment = new();
    readonly Dictionary<EquipmentType, EquipmentIcon> iconDict = new();

    // run-time
    private List<Equipment> currentEquipment = new();

    void Start()
    {
        instance = this;
        
        // set up iconDict
        foreach (var icon in icons)
        {
            if (iconDict.ContainsKey(icon.type))
                Debug.LogException(new Exception($"EquipmentManager has duplicate icons for {icon.type}!"));
            
            iconDict[icon.type] = icon;
        }

        // ensure every EquipmentType has an icon
        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            if (!iconDict.ContainsKey(type))
                Debug.LogException(new Exception($"EquipmentManager has not icon for {type}!"));
        }
        
        FinalizeEquipmentList();
    }

    /**
     * Called in Start. Brings allWeapons into allEquipment and adds all equipment not able to be changed from the inspector, such as passives.
     */
    void FinalizeEquipmentList()
    {
        allEquipment.AddRange(allWeapons);
        allEquipment.Add(new FairyFriendPassive());
        allEquipment.Add(new BunnyBurstPassive());
    }


    void Update()
    {
        foreach (var item in currentEquipment)
        {
            item.Update();
        }
        
        // TEMP
        if (Input.GetKeyDown(KeyCode.P))
        {
            LevelUpUI.instance.Open();
        }
    }

    /**
     * Randomly picks and returns the four options available on level-up.
     * These can be either new equipment or an upgrade to old equipment.
     * Called when the LevelUpUI is instantiated.
     */
    public List<UpgradeOption> GetUpgradeOptions()
    {
        var options = new List<UpgradeOption>();
        var weaponCount = currentEquipment.Count(e => e is Weapon);
        var passiveCount = currentEquipment.Count(e => e is Passive);

        foreach (var equipment in allEquipment)
        {
            if (equipment is Weapon && weaponCount >= MAX_WEAPONS) continue;
            if (equipment is Passive && passiveCount >= MAX_PASSIVES) continue;

            var duplicate = currentEquipment.FirstOrDefault(e => e == equipment);
            if (duplicate != null) // equipment is already in use. present level-up instead
            {
                if (duplicate.levelUpsDone >= MAX_EQUIPMENT_LEVELS) continue; // already max level
                options.Add(new UpgradeOption(equipment, true, GetLevelUps(equipment)));
            }
            else // present new equipment
            {
                options.Add(new UpgradeOption(equipment, false, null));
            }
        }

        return options.OrderBy(e => Random.Range(0f, 1f)).Take(4).ToList();
    }

    public EquipmentIcon GetIcon(Equipment equipment)
    {
        return iconDict[equipment.type];
    }

    WeaponLevelUp[] GetLevelUps(Equipment equipment)
    {
        if (equipment is Passive) return null;
        else if (equipment is Weapon weapon)
        {
            return weapon.levelUpOptions.OrderBy(e => Random.Range(0f, 1f)).Take(2).ToArray();
        }
        else throw new Exception("Invalid equipment type!");
    }

    /**
     * After presenting the options from GetUpgradeOptions, when the player selects one, this function is called to apply it.
     */
    public void ApplyUpgradeOption(UpgradeOption upgrade)
    {
        if (upgrade.isLevelUp)
        {
            switch (upgrade.equipment)
            {
                case Weapon weapon:
                {
                    foreach (var levelUp in upgrade.levelUps)
                        weapon.ApplyLevelUp(levelUp);
                    break;
                }
                case Passive passive:
                    passive.ApplyLevelUp();
                    break;
                default:
                    Debug.LogError($"Unexpected equipment type `{upgrade.equipment.GetType()}`!");
                    break;
            }
        }
        else
        {
            this.currentEquipment.Add(upgrade.equipment);
            upgrade.equipment.OnEquip();

            foreach (var prevEquipment in currentEquipment)
            {
                if (prevEquipment == upgrade.equipment) continue;
                
                upgrade.equipment.ProcessOther(prevEquipment);
            }
        }
    }
}

/**
 * Holds an option presented to the player when they level up.
 * Can be either a weapon, a passive, or an upgrade to an existed item.
 */
public class UpgradeOption
{
    public Equipment equipment;
    
    /** True if this is upgrading an already-existing equipment. */
    public bool isLevelUp;

    /**
     * If isUpgrade is true AND equipment is a weapon, this will hold the two improvements to apply to the weapon.
     * else, it will be NULL.
     */
    public WeaponLevelUp[] levelUps;

    public UpgradeOption(Equipment equipment, bool isLevelUp, WeaponLevelUp[] levelUps)
    {
        this.equipment = equipment;
        this.isLevelUp = isLevelUp;
        this.levelUps = levelUps;
    }
}