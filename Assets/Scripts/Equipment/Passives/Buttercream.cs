using System;
using UnityEngine;

/*
Buttercream
* Increases Max Health
* Gives a chance to heal from killing enemies.
* Synergizes with Cupquake
* Static Upgrade Path
    * Increases Max Health and % chance to heal
*/

class Buttercream : Passive
{
    class State {
        public float healthIncrease = 0;
        public float healChance = 0;
    }

    [Tooltip("How much the passive increases max health (measured in HP)")]
    public float maxHealthBuff = 40.0f;

    [Tooltip("The chance a player will heal after killing an enemy")]
    public float healChance = 0.05f;

    [Tooltip("The amount of health the player gains when killing an enemy.")]
    public float lifestealAmount = 50.0f;

    State state = new();
    PlayerHealth ph;


    public override (string description, Action onApply) GetLevelUps() {
        return ("Increase heal on kill chance and max HP", apply);
    }

    void Initalize() {
        ph = Player.instance.GetComponent<PlayerHealth>();
        EnemyTemplate.EnemyDied += () => {
            if (enabled) { // double check that this passive is active
                if (UnityEngine.Random.Range(0f, 1f) < state.healChance) {
                    ph.increaseHealth(lifestealAmount);
                }
            }
        };
    }

    public override void OnEquip() { 
        Initalize();
        apply(); 
    }

    void apply() {
        ph.maxHealth += maxHealthBuff;
        ph.increaseHealth(maxHealthBuff);

        state.healthIncrease += maxHealthBuff;
        state.healChance += healChance;
    }

    public override void OnUnEquip() { }

    protected override object FreezeRaw() {
        return state;
    }

    protected override void Thaw(object data) {
        Initalize();
        state = (State)data;

        ph.maxHealth += state.healthIncrease;
        ph.increaseHealth(state.healthIncrease);
    }
}
