using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    const int MAX_WEAPONS = 5;
    const int MAX_PASSIVES = 4;
    const int MAX_EQUIPMENT_LEVELS = 6;
    
    
    // in inspector
    public EquipmentIcon[] icons;
    [SerializeField] public ProjectileWeapon[] allWeapons;

    // general
    public static EquipmentManager instance;
    [HideInInspector]
    public List<Equipment> allEquipment = new();

    // run-time
    private List<Equipment> currentEquipment = new();

    /// <summary>
    /// Saves the current equipment to disk.
    /// </summary>
    public void Freeze() {
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

    bool thawed = false;
    /// <summary>
    /// Restores weapons between scene changes / disk. This should only be called once.
    /// </summary>
    // NOTE: Thaw needs to happen after Player.Awake and before InGameUI.Start (ie GetUpgradeOptions(true))
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

        InGameUI.UpdateItems();
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

    /// <summary>
    /// Gets a random number of elements from input and returns a new list from them.
    /// Effectively returns the list shuffled if `input.Count <= n`.
    /// </summary>
    public static List<T> takeRandom<T>(List<T> input, int n) {
        var bag = new List<T>(input);
        var result = new List<T>();

        for (int i = 0; i < n; i++) {
            if (bag.Count == 0) { break; }

            var index = Random.Range(0, bag.Count);
            result.Add(bag[index]);
            bag.RemoveAt(index);
        }

        return result;
    }

    /// <summary>
    /// Retrieves options for hell's curse.
    /// Returns an empty list (if the player has only one weapon),
    /// a list with the player's highest level weapon, 
    /// or 1-4 of the weapons meeting the <paramref name="minLevel"/> constraint.
    /// </summary>
    public List<UpgradeOption> GetHellsCurseOptions(int minLevel) 
    {
        Thaw();

        // Don't destroy the player's last weapon.
        if (currentEquipment.OfType<Weapon>().Count() == 1) {
            return new();
        }

        // Get 4 random weapons that satisfy the level requirement
        var weapons = takeRandom(currentEquipment
            .OfType<Weapon>()
            .Where(e => e.levelUpsDone >= minLevel - 1)
            .ToList(), 4); 

        // If no weapons satisfy the level requirement, then pick the highest level one.
        if (weapons.Count == 0) {
            weapons = currentEquipment
                .OfType<Weapon>()
                .OrderByDescending(e => e.levelUpsDone)
                .Take(1)
                .ToList();
        }

        // Map the weapons to upgrade options that remove the weapon when selected.
        return weapons.Select(e => new UpgradeOption(
            e.icon.name,
            e.icon.icon,
            $"Level: {e.levelUpsDone + 1}",
            () => RemoveEquipment(e),
            e.icon.titleFontSize
        )).ToList();
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
            // enforce first show rules 
            if (equipment is Weapon)
                if (!(equipment as Weapon).availableAtStart && firstShow)
                    continue;
            if (equipment is Passive)
            {
                if (firstShow) continue;
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

                    // Inform the other equipment about the upgrade
                    foreach (var e in currentEquipment) {
                        if (e == equipment)
                        e.ProcessOther(equipment);
                    }

                };
                options.Add(new UpgradeOption(icon.name, icon.icon, description, onApply, icon.titleFontSize));
            }
            else // present new equipment
            {
                // only present new equipment if there's enough space
                if (equipment is Passive && passiveCount >= MAX_PASSIVES) { continue; }
                if (equipment is Weapon && weaponCount >= MAX_WEAPONS) { continue; }
                Action onApply = () => AddNewEquipment(equipment);
                options.Add(new UpgradeOption(icon.name, icon.icon, icon.description, onApply, icon.titleFontSize));
            }
        }

        return options.OrderBy(_ => Random.Range(0f, 1f)).Take(4).ToList();
    }

    private void RemoveEquipment(Equipment equipment) {
        currentEquipment.Remove(equipment);
        equipment.OnUnEquip();
        equipment.enabled = false;
        equipment.levelUpsDone = 0;

        foreach (var prevEquipment in currentEquipment)
        {
            if (prevEquipment == equipment) continue;
            
            equipment.ProcessOtherRemoval(prevEquipment);
        }
    }

    private void AddNewEquipment(Equipment equipment)
    {
        this.currentEquipment.Add(equipment);
        equipment.OnEquip();
        equipment.enabled = true;
        
        foreach (var prevEquipment in currentEquipment)
        {
            if (prevEquipment == equipment) continue;
            
            if (!(equipment is FlocksOfAFeather)) { // avoid double spawning feathers
                equipment.ProcessOther(prevEquipment);
            }

            prevEquipment.ProcessOther(equipment);
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
    public int titleFontSize;


    public UpgradeOption(string name, Texture icon, string description, Action onSelect, int titleFontSize)
    {
        this.name = name;
        this.icon = icon;
        this.description = description;
        this.onSelect = onSelect;
        this.titleFontSize = titleFontSize;
    }
}
