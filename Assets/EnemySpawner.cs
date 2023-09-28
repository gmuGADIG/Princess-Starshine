using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //spawn an enemy
    //have public variables to adjust spawn rate, acceleration, enemy type

    public float spawnRate = 1.0f; //seconds per spawn
    public float spawnAcceleration = 0.0f; //the amount that the time between spawns decreases per spawn
    public GameObject enemyType;
    private float countdownTimer;


    public void SpawnEnemy()
    {
        Instantiate(enemyType, transform.position, Quaternion.identity);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        countdownTimer = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        countdownTimer -= Time.deltaTime;
        if (countdownTimer <= 0)
        {
            SpawnEnemy();
            spawnRate -= spawnAcceleration; //decrease spawnRate by spawnAcceleration
            countdownTimer = spawnRate;
        }
    }
}
