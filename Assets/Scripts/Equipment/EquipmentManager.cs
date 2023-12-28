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
    [SerializeField] public ProjectileWeapon[] allWeapons;

    // general
    public static EquipmentManager instance;
    [HideInInspector]
    public List<Equipment> allEquipment = new();

    // run-time
    private List<Equipment> currentEquipment = new();

    void OnDestroy() {
        SaveManager.SaveData.frozenEquipment = currentEquipment.Select(e => e.Freeze()).ToList();
    }

    void Awake() {
        instance = this;
        
        foreach (Equipment equipment in GetComponents<Equipment>()) {
            equipment.enabled = false;
            allEquipment.Add(equipment);
        }

        ProjectileWeapon.staticStatModifiers = new();
    }

    // NOTE: Thaw needs to happen after Player.Awake and before InGameUI.Start (ie GetUpgradeOptions(true))
    /// <summary>
    /// Restores weapons between scene changes / disk. This should only be called once.
    /// </summary>
    bool thawed = false;
    public void Thaw() {
        if (thawed) { 
            Debug.LogWarning("EquipmentManager.Thaw called more than once!");
            return; 
        }

        currentEquipment = new();
        foreach (var frozen in SaveManager.SaveData.frozenEquipment) {
            // find "alive" equipment
            var equipment = allEquipment.Where(e => e.GetType().ToString() == frozen.Type).ToArray()[0];
            equipment.Thaw(frozen);

            currentEquipment.Add(equipment);
            equipment.enabled = true;
        }

        thawed = true;
    }

    void Update()
    {
        /*
        foreach (var item in currentEquipment)
        {
            item.Update();
        }
        */
        
        // TEMP
        if (Input.GetKeyDown(KeyCode.P) && Application.isEditor)
        {
            if (LevelUpUI.instance == null) Debug.LogError("Can't open level-up ui; none exists in this scene!");
            else LevelUpUI.instance.Open();
        }
    }

    /**
     * Randomly picks and returns the four options available on level-up.
     * These can be either new equipment or an upgrade to old equipment.
     * Called when the LevelUpUI is instantiated.
     */
    public List<UpgradeOption> GetUpgradeOptions(bool firstShow = false)
    {
        if (firstShow) {
            Thaw();
        }

        var options = new List<UpgradeOption>();
        var weaponCount = currentEquipment.Count(e => e is Weapon);
        var passiveCount = currentEquipment.Count(e => e is Passive);

        foreach (var equipment in allEquipment)
        {
            // enforce first show rules and max weapon and max passive count
            if (equipment is Weapon && weaponCount >= MAX_WEAPONS) continue;
            if (equipment is Weapon)
                if (!(equipment as Weapon).availableAtStart && firstShow)
                    continue;
            if (equipment is Passive)
            {
                if (firstShow) continue;
                if (passiveCount >= MAX_PASSIVES) continue;
            }

            var icon = equipment.icon;
            
            var duplicate = currentEquipment.FirstOrDefault(e => e == equipment);

            if (duplicate != null) // equipment is already in use. present level-up instead
            {
                if (duplicate.levelUpsDone >= MAX_EQUIPMENT_LEVELS) continue; // already max level
                var (description, applyLevelUp) = equipment.GetLevelUps();
                Action onApply = () =>
                {
                    equipment.levelUpsDone += 1;
                    applyLevelUp();
                };
                options.Add(new UpgradeOption(icon.name, icon.icon, description, onApply));
            }
            else // present new equipment
            {
                Action onApply = () => AddNewEquipment(equipment);
                options.Add(new UpgradeOption(icon.name, icon.icon, icon.description, onApply));
            }
        }

        return options.OrderBy(_ => Random.Range(0f, 1f)).Take(4).ToList();
    }

    private void AddNewEquipment(Equipment equipment)
    {
        this.currentEquipment.Add(equipment);
        equipment.OnEquip();
        equipment.enabled = true;
        
        foreach (var prevEquipment in currentEquipment)
        {
            if (prevEquipment == equipment) continue;
            
            equipment.ProcessOther(prevEquipment);
        }
    }

    public List<Texture> EquippedWeaponIcons()
    {
        return currentEquipment.Where(e => e is Weapon).Select(e => e.icon.icon).ToList();
    }
    
    public List<Texture> EquippedPassiveIcons()
    {
        return currentEquipment.Where(e => e is Passive).Select(e => e.icon.icon).ToList();
    }
}

/**
 * Holds an option presented to the player when they level up. Can be either a new equipment or an item level-up.
 * onSelect is called when the user selects this upgrade, and should add the equipment or level-up the item.
 */
public class UpgradeOption
{
    public string name;
    public Texture icon;
    public string description;
    public Action onSelect;


    public UpgradeOption(string name, Texture icon, string description, Action onSelect)
    {
        this.name = name;
        this.icon = icon;
        this.description = description;
        this.onSelect = onSelect;
    }
}