using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterPrimary : Projectile
{   
    private GameObject explodePrefab;
    private float timeAlive = 0;

    private void Start()
    {
        if (explodePrefab == null)
        {
            explodePrefab = Resources.Load<GameObject>("Projectiles/GlitterBomb/GlitterExplode");
        }
    }

    void Update()
    {
        base.Update();
        timeAlive += Time.deltaTime;
        if (timeAlive >= 1.5f)
        {
            Explode();
        }
    }

    void Explode()
    {
        Object.Instantiate(explodePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}