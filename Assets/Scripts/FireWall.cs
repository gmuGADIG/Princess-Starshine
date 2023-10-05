using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    //The sparks that will come out of the wall of fire
    public GameObject fireSparks;
    //Size Y of the range that sparks can spawn
    [SerializeField] private float sizeY = 5f;
    //Size X of the range that sparks can spawn
    [SerializeField] private float sizeX = 2f;
    //Rate at which sparks will spawn, the lower the value the faster
    [SerializeField] private float sparkRate = 0.1f;

    private void Start()
    {
        InvokeRepeating("spawnSparks", 0.1f, sparkRate);
    }

    //Randomly spawn sparks of fire of adjustable sizes
    void spawnSparks()
    {
        
        Vector3 pos = new Vector3(transform.position.x + UnityEngine.Random.Range(-sizeX, sizeX), transform.position.y + UnityEngine.Random.Range(-sizeY, sizeY));
        GameObject fire = Instantiate(fireSparks, pos, Quaternion.identity);
    }

}
