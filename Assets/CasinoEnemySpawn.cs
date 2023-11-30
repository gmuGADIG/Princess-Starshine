using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoEnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    private Vector3 spawnPos;
    private float timer;
    private float timesRan = 0;
    private Camera cam;

    public float timesToRun = 3;
    public float groupSize = 4;
    public float spawnDelay = 1;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnDelay)
        {
            timer = 0;
            for (int i = 0; i < groupSize; i++)
            {
                spawn(i);
            }
            timesRan++;
            if (timesRan >= timesToRun)
            {
                this.enabled = false;
            }
        }

    }
    void spawn(float location)
    {
        if (location == 0)
        {
            spawnPos = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));
        }
        if (location == 1)
        {
            spawnPos = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        }
        if (location == 2)
        {
            spawnPos = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        }
        if (location == 3)
        {
            spawnPos = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        }
        Instantiate(enemy, spawnPos, Quaternion.identity);
    }

}
