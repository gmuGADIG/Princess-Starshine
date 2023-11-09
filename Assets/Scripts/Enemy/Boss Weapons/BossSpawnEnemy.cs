using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public Transform enemyPos;
    private Vector3 tempPos;
    private float timer;
    private float timesRan;

    public float timesToRun = 5;
    public float groupSize = 5;
    public float spawnDelay = 1;
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnDelay)
        {
            timer = 0;
            for (int i = 0; i < groupSize; i++) {
                spawn();
            }
            timesRan++;
            if (timesRan >= timesToRun)
            {
                this.enabled = false;
            }
        }

    }
    void spawn()
    {
        tempPos = new Vector3(Random.Range(enemyPos.position.x - 1, enemyPos.position.x + 1), Random.Range(enemyPos.position.y - 1, enemyPos.position.y + 1));
        Instantiate(enemy, tempPos, Quaternion.identity);
    }
}
