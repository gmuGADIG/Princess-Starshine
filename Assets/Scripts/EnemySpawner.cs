using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{

    //spawn an enemy
    //have public variables to adjust spawn rate, acceleration, enemy type

    [Tooltip("How fast enemies spawn at the start of the level, in spawns per second.")]
    public float startSpawnRate = 1.0f;
    [Tooltip("How fast enemies spawn at the end of the level, in spawns per second. Spawn rate will linearly approach this value as the level plays.")]
    public float endSpawnRate = 5.0f;
    [Tooltip("How long the level is, which determines the time at which endSpawnRate should be used.")]
    public float levelSeconds = 50;
    
    [System.Serializable]
    public struct EnemySpawn
    {
        public GameObject enemyType;
        public float probabilityWeight;
        public int spawnCount;
    }
    public EnemySpawn[] enemySpawns;
    //public GameObject[] enemiesToSpawn;
    //public float[] probabilityWeight;


    float startTime;
    float timeTillNextSpawn = 0;

    public static UnityEvent SpawningEnemy = new UnityEvent();

    public void RandomSpawn()
    {
        SpawningEnemy.Invoke();
        
        // cumulativeWeights[i] = sum of weights 0 to i
        float[] cumulativeWeights = new float[enemySpawns.Length];
        for (int i = 0; i < enemySpawns.Length; i++)
        {
            var prevWeight = cumulativeWeights.ElementAtOrDefault(i - 1);
            cumulativeWeights[i] = prevWeight + enemySpawns[i].probabilityWeight;
        }
        
        // select a random number less than the sum of the weights
        float rand = Random.Range(0, cumulativeWeights[^1]);
        // find the first index where sum_of_weights(0 to index) is greater than rand
        int selectedIndex = cumulativeWeights.ToList().FindIndex(n => n > rand);
        
        var selectedSpawn = enemySpawns[selectedIndex];
        Spawn(selectedSpawn);
    }

    private void Spawn(EnemySpawn spawn)
    {
        for (int i = 0; i < spawn.spawnCount; i++)
            Instantiate(spawn.enemyType, GetSpawnPosition(), Quaternion.identity);
    }

    private Vector2 GetSpawnPosition()
    {
        var camPos = Camera.main.transform.position;

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * ((float)Screen.width / Screen.height);

        const float distanceFromBounds = 1; // how far past the bounds the enemy's center will spawn at
        
        // randomly spawn at the right, bottom, or top wall, weighted by the lengths of those walls
        var rand = Random.Range(0, halfHeight + halfWidth + halfWidth);
        if (rand < halfHeight) // spawn at right wall
        {
            return camPos + Vector3.right * (halfWidth + distanceFromBounds) + Vector3.up * Random.Range(-halfHeight, halfHeight);
        }
        else if (rand < halfHeight + halfWidth) // spawn at bottom wall
        {
            return camPos + Vector3.down * (halfHeight + distanceFromBounds) + Vector3.right * Random.Range(-halfWidth, halfWidth);
        }
        else // spawn at top wall
        {
            return camPos + Vector3.up * (halfHeight + distanceFromBounds) + Vector3.right * Random.Range(-halfWidth, halfWidth);
        }

    }

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        timeTillNextSpawn -= Time.deltaTime;
        if (timeTillNextSpawn <= 0)
        {
            RandomSpawn();
            var t = (Time.time - startTime) / levelSeconds;
            var rate = Mathf.Lerp(startSpawnRate, endSpawnRate, t);
            timeTillNextSpawn += 1 / rate;
            
            // print($"Spawning Enemies! (t = {t}, rate = {rate})");
        }

        // countdownTimer -= Time.deltaTime;
        // spawnChangeTimer -= Time.deltaTime;
        // if (countdownTimer <= 0)
        // {
        //     SpawnEnemy();
        //     countdownTimer = spawnRate;
        // }
        // if (spawnChangeTimer <= 0)
        // {
        //     if (spawnRate < minSpawnRate)
        //     {
        //         spawnRate = minSpawnRate; //spawn rate has reached its minimum
        //     }
        //     else
        //     {
        //         spawnRate -= spawnAcceleration; //decrease spawnRate by spawnAcceleration
        //     }
        //     
        // }
    }
}
