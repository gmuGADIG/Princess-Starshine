using System;
using Unity.VisualScripting;
using UnityEngine;

public class FairyFriendPassive : Passive {
    //Amount of health healed per tick
    public float healAmount = 2f;

    //Amount of health healed per tick
    public float healAmountUpgrade = 1f;

    //The Player prefab
    private GameObject playerPrefab;
    private PlayerHealth health;

    public void Start() {
        //playerPrefab = GameObject.FindGameObjectWithTag("Player");
        //health = playerPrefab.GetComponent<PlayerHealth>();
    }

    public override void Update() {
        /* Finds the player by finding the tag "Player" and then increasing the player 
         * health by healAmount if the PlayerHealth component exists on the player
         */
            if (health != null) {
                health.increaseHealth(healAmount * Time.deltaTime);
                //Debug.Log(health.tempHealth);
            }
            else {
                //Debug.LogError("Player is missing PlayerHealth component");
            }
    }

    /* Increases the healAmount by levelAmountUpgrade and decreases cooldownRate
     * by cooldownRate Upgrade until cooldown cap is hit
     */
    public void levelUp() {
        healAmount += healAmountUpgrade;
    }

    public override void OnEquip() {
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        health = playerPrefab.GetComponent<PlayerHealth>();
    }

    public override void OnUnEquip()  {
        
    }

    //Applies the levelUp() method when leveling up
    public override (string description, Action onApply) GetLevelUps() {
        return ("Greater Health Regen", levelUp);
    }
    protected override object FreezeRaw() { return healAmount; }
    protected override void Thaw(object data) {
        healAmount = (float)data;
        OnEquip();
    }
}
