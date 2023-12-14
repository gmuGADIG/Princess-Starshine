using System;
using UnityEngine;


/**
 * A single piece of equipment (weapon or passive).
 * Is instantiated after the player equips it.
 */
[Serializable]
public abstract class Equipment : MonoBehaviour
{
    [SerializeField] public EquipmentIcon icon;
    
    /** The amount of times this equipment has been leveled up. Necessary to avoid going past the limit */
    [HideInInspector] public int levelUpsDone = 0;

    /**
     * Called when this item is equipped. Should set the buffs of the item.
     * Guarenteed to be called before Update().
     */
    public abstract void OnEquip();

    /**
     * Called when this item is removed from the player. Should undo anything done in OnEquip.
     */
    public abstract void OnUnEquip();
    
    /**
     * Called every frame. Handles anything that happens throughout the gameplay, such as weapons firing.
     */
    public abstract void Update();
    
    /**
     * Add synergy bonuses.
     * After equipping an item, this is called for each other equipment the player has.
     * Also called on the newly-equipped item for each previous item.
     */
    public abstract void ProcessOther(Equipment other);

    /**
     * Undo synergy bonuses here.
     * After removing an item, this is called for each other equipment the player has.
     */
    public abstract void ProcessOtherRemoval(Equipment other);

    /**
     * Called when the player has the option to level-up the weapon.
     * Do not change the weapon when called. Only do that in the onApply function, which is called if the level-up is selected.
     * If something else is selected, nothing should happen.
     * This function can be non-deterministic, as is the case with weapon upgrades.
     */
    public abstract (string description, Action onApply) GetLevelUps();

    /// <summary>
    /// Called when this equipment is thawed. 
    /// This will be called in place of "lifecycle" methods like OnEquip and ProcessOther.
    /// </summary>
    /// <param name="equipment">The older version of this equipment. Should be safe to downcast.</param>
    public abstract void Thaw(Equipment equipment);
}