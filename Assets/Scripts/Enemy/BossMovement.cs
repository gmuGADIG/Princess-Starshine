using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyTemplate
{
    /*
    private GameObject findEnemy()
    {
        float randomThing = Random.Range(0, 10);
        return ;
    }
    */

    protected override void Start()
    {

        if (moveTowardsObject == null) { moveTowardsObject = GameObject.FindGameObjectWithTag("Player"); }
        // if (XPOrb == null) { XPOrb = GameObject.FindGameObjectWithTag("XPOrb"); }
        CurrentHealth = MaxHealth;
        CheckDeath();
    }
}
