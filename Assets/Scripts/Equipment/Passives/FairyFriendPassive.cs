using System;
using UnityEngine;

public class FairyFriendPassive : Passive
{
    //Amount of health healed per tick
    public float healAmount = 10f;

    //Cooldown between each heal
    public float cooldownRate = 2.5f;

    //Amount of health healed per tick
    public float healAmountUpgrade = 5f;

    //Cooldown between each heal
    public float cooldownRateUpgrade = 0.25f;

    //Current cooldown timer
    private float cooldown;

    //The Player prefab
    private GameObject playerPrefab;

    public void Start()
    {
        cooldown = cooldownRate;
    }

    public override void Update()
    {
        cooldown -= Time.deltaTime;

        /* Finds the player by finding the tag "Player" and then increasing the player 
         * health by healAmount if the PlayerHealth component exists on the player
         */
        if(cooldown <= 0)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                playerPrefab = GameObject.FindGameObjectWithTag("Player");
                if (playerPrefab.GetComponent<PlayerHealth>() != null)
                {
                    PlayerHealth health = playerPrefab.GetComponent<PlayerHealth>();

                    health.increaseHealth(healAmount);
                    Debug.Log(health.tempHealth);
                }
                else
                {
                    Debug.LogError("Player is missing PlayerHealth component");
                }
            }
            else
            {
                Debug.LogError("Player is missing 'Player' tag");
            }
            cooldown = cooldownRate;
        }
    }

    /* Increases the healAmount by levelAmountUpgrade and decreases cooldownRate
     * by cooldownRate Upgrade until cooldown cap is hit
     */
    void levelUp()
    {
        healAmount += healAmountUpgrade;
        if (cooldownRate > cooldownRateUpgrade)
        {
            cooldownRate -= cooldownRateUpgrade;
        }
    }

    public override void OnEquip()
    {

    }

    public override void OnUnEquip() 
    {
        
    }

    //Applies the levelUp() method when leveling up
    public override (string description, Action onApply) GetLevelUps()
    {
        return ("Greater Health Regen", levelUp);
    }
}
