using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossRandoMovement : EnemyTemplate {
    [Header("Boss Rando Movement")]
    public float TimeBetweenMoves = 3f;
    private float timer;
     
    protected override void Start() {
        base.Start();
        timer = TimeBetweenMoves;
    }
    protected override void Update() {
        if (timer <= 0) {
            timer = TimeBetweenMoves;
            //works, but probably slow
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
            if (objects.Length == 0) {
                //in event of no enemies, move towards player
                objects = GameObject.FindGameObjectsWithTag("Player");
            }
            moveTowardsObject = objects[Random.Range(0, objects.Length)];
        }
        else {
            timer -= Time.deltaTime;
        }
        //Update normally moves towards object
        base.Update();
    }
}
