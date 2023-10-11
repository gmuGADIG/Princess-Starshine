using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterExplode : MonoBehaviour
{   
    private float timeAlive = 0;

    [SerializeField]
    private int damage = 10;

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= 0.25f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}