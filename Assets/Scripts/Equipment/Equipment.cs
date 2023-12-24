using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.Assertions;

/**
 * A single piece of equipment (weapon or passive).
 * Is instantiated after the player equips it.
 */
[Serializable]
public abstract class Equipment : MonoBehaviour
{
    [SerializeField] public EquipmentIcon icon;
    
    /// <summary>
    /// The amount of times this equipment has been leveled up. 
    /// Necessary to avoid going past the limit.
    /// This value is one lower than the "level" of the weapon.
    /// </summary>
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

    public FrozenEquipment Freeze() {
        return new FrozenEquipment {
            Type = GetType().ToString(),
            LevelUpsDone = levelUpsDone,
            Data = FreezeRaw()
        };
    }

    public void Thaw(FrozenEquipment frozen) {
        if (frozen.Type == GetType().ToString()) {
            levelUpsDone = frozen.LevelUpsDone;
            var data = frozen.Data;

            if (data is JObject) {
                var parsed = JsonConvert.DeserializeObject(data.ToString(), FreezeRaw().GetType());
                Thaw(parsed);
            } else if (data is double && FreezeRaw() is float) {
                Thaw((float)(double)data); // only sane code written here
            }
            else {
                Thaw(data);
            }
        } else {
            throw new ArgumentException("frozen.Type != GetType().ToString()");
        }
    }

    /// <summary>
    /// Called when the state of an equipment needs to be saved/carried between scenes.
    /// This function may also be called for type information, so it needs to handle invalid states.
    /// </summary>
    /// <returns>Data representing the state of this equipment</returns>
    protected abstract object FreezeRaw();

    /// <summary>
    /// Called when this equipment is thawed. 
    /// This will be called in place of lifecycle methods like OnEquip and ProcessOther.
    /// </summary>
    /// <param name="data">The data returned from FreezeRaw()</param>
    protected abstract void Thaw(object data);
}