using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //spawn an enemy
    //have public variables to adjust spawn rate, acceleration, enemy type

    public float spawnRate = 1.0f; //seconds per spawn
    public float spawnAcceleration = 1.0f; //the amount that the time between spawns increases per spawn
    public float spawnChangeInterval = 5.0f; //after this many seconds have passed, the spawn rate will increase
    public float minSpawnRate = 0.1f; //the shortest amount of time between spawns

    [System.Serializable]
    public struct EnemySpawn
    {
        public GameObject enemyType;
        public float probabilityWeight;
    }
    public EnemySpawn[] enemySpawns;
    //public GameObject[] enemiesToSpawn;
    //public float[] probabilityWeight;


    private float countdownTimer;
    private float spawnChangeTimer;

    
    


    public void SpawnEnemy()
    {
        float totalWeights = 0.0f;
        foreach (EnemySpawn i in enemySpawns)
        {
            totalWeights += i.probabilityWeight;
        }

        float spawnRando = Random.Range(0, totalWeights);

        for (int i = 0; i < enemySpawns.Length; i++)
        {
            spawnRando -= enemySpawns[i].probabilityWeight;
            if (spawnRando < 0)
            {
                if (enemySpawns[i].enemyType == null)
                {
                    Debug.LogError("EnemySpawner \"" + name + "\": EnemySpawn " + i + " has no enemyType assigned! Skipping spawn");
                    return;
                }
                Instantiate(enemySpawns[i].enemyType, transform.position, Quaternion.identity);
                break;
            }
        }

        //Instantiate(enemyType, transform.position, Quaternion.identity);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        countdownTimer = spawnRate;
        spawnChangeTimer = spawnChangeInterval;

    }

    // Update is called once per frame
    void Update()
    {
        countdownTimer -= Time.deltaTime;
        spawnChangeTimer -= Time.deltaTime;
        if (countdownTimer <= 0)
        {
            SpawnEnemy();
            countdownTimer = spawnRate;
        }
        if (spawnChangeTimer <= 0)
        {
            if (spawnRate < minSpawnRate)
            {
                spawnRate = minSpawnRate; //spawn rate has reached its minimum
            }
            else
            {
                spawnRate -= spawnAcceleration; //decrease spawnRate by spawnAcceleration
            }
            
        }
    }
}
