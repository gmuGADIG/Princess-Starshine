using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NewBehaviourScript : EnemyTemplate
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
