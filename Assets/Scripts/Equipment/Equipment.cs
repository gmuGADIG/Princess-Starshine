using System;
using UnityEngine;


/**
 * A single piece of equipment (weapon or passive).
 * Is instantiated after the player equips it.
 */
[Serializable]
public abstract class Equipment
{
    [SerializeField] public EquipmentType type;
    
    /** The amount of times this equipment has been leveled up. Necessary to avoid going past the limit */
    [HideInInspector] public int levelUpsDone = 0;

    /**
     * Called when this item is equipped. Should set the buffs of the item.
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
}