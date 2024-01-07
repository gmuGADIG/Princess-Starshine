using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{

    //spawn an enemy
    //have public variables to adjust spawn rate, acceleration, enemy type

    [Tooltip("How fast enemies spawn at the start of the level, in spawns per second.")]
    public float startSpawnRate = 1.0f;
    [Tooltip("How fast enemies spawn towards the end of the level, in spawns per second. Spawn rate will linearly approach this value as the level plays.")]
    public float endSpawnRate = 5.0f;

    [Tooltip("How long the level is, which determines when the boss spawns and how quickly the spawn rate approaches endSpawnRate")]
    public float levelSeconds = 50;

    [Tooltip("The scene swap to after the time ends, unless this string is empty, in which case nothing happens and spawning continues.")]
    public string bossSceneOrEmpty;

    [System.Serializable]
    public struct EnemySpawn
    {
        public GameObject enemyType;
        public float probabilityWeight;
        public int spawnCount;

        public float startingDamage;
        public float endingDamage;
    }
    public EnemySpawn[] enemySpawns;
    
    float startTime;
    float timeTillNextSpawn = 0;

    public static UnityEvent SpawningEnemy = new UnityEvent();

    Wall wall;

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
        for (int i = 0; i < spawn.spawnCount; i++) {
            GameObject enemy = Instantiate(spawn.enemyType, GetSpawnPosition(), Quaternion.identity);
            if (enemy.TryGetComponent(out Damage damage))
            {
                float t = (Time.time - startTime) / levelSeconds;
                damage.damage = Mathf.Lerp(spawn.startingDamage, spawn.endingDamage, t);
            }
        }
    }

    private Vector2 GetSpawnPosition()
    {
        var camPos = Camera.main.transform.position;

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * ((float)Screen.width / Screen.height);

        const float distanceFromBounds = 1; // how far past the bounds the enemy's center will spawn at
        
        // randomly spawn at the right, bottom, or top wall, weighted by the lengths of those walls
        if (wall == null) {
            var rand = Random.Range(0, halfHeight + halfWidth + halfWidth);
            if (rand < halfHeight) // spawn at right wall
            {
                return camPos + Vector3.right * (halfWidth + distanceFromBounds) + Vector3.up * Random.Range(-halfHeight, halfHeight);
            }
            else if (rand < halfHeight + halfWidth) // spawn at bottom wall (right half)
            {
                return camPos + Vector3.down * (halfHeight + distanceFromBounds) + Vector3.right * Random.Range(0, halfWidth);
            }
            else // spawn at top wall (right half)
            {
                return camPos + Vector3.up * (halfHeight + distanceFromBounds) + Vector3.right * Random.Range(0, halfWidth);
            }
        } else {
            var rand = Random.Range(0, halfHeight + halfWidth + halfWidth);
            if (rand < halfHeight) // spawn at right wall
            {
                var maxY = wall.Border - camPos.y;
                return camPos + Vector3.right * (halfWidth + distanceFromBounds) + Vector3.up * Random.Range(-halfHeight, maxY);
            }
            else // spawn at bottom wall (right half)
            {
                return camPos + Vector3.down * (halfHeight + distanceFromBounds) + Vector3.right * Random.Range(0, halfWidth);
            }
        }

    }

    void Start()
    {
        startTime = Time.time;
        wall = FindObjectOfType<Wall>();
    }

    void Update()
    {
        var secondsSinceStart = Time.time - startTime;
        var bossShouldSpawn = secondsSinceStart > levelSeconds;

        if (bossShouldSpawn && bossSceneOrEmpty != "")
        {
            print("SPAWNING BOSS!");
            SceneManager.LoadScene(bossSceneOrEmpty);

            EquipmentManager.instance.Freeze();
            Player.instance.Freeze();

            return;
        }
        
        timeTillNextSpawn -= Time.deltaTime;
        if (timeTillNextSpawn <= 0)
        {
            if (EnemyManager.enemyManager.enemies.Count <= EnemyManager.enemyManager.maxEnemies)
            {
                RandomSpawn();
            }

            float t = (Time.time - startTime) / levelSeconds;
            float rate = Mathf.Lerp(startSpawnRate, endSpawnRate, t);
            
            timeTillNextSpawn += 1 / rate;
        }
    }
}
