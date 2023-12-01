using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessEnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform enemyPos;
    private Vector3 tempPos;
    private float timer;
    private float timesRan = 0;
    private Camera cam;
    private float enemyType;

    public float timesToRun = 1;
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
        enemyType = Random.Range(0f, 2f);
        if(enemyType >= 1)
        {
            Debug.Log("Enemy #" + (location + 1) + " is a different enemy");
        }
        else
        {
            Debug.Log("Enemy #" + (location + 1) +  " is a Goon");
        }
        if(location == 0)
        {
            tempPos = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));
        }
        if (location == 1)
        {
            tempPos = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        }
        if (location == 2)
        {
            tempPos = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        }
        if (location == 3)
        {
            tempPos = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        }
        Instantiate(enemy, tempPos, Quaternion.identity);
    }

}
