using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterPrimary : MonoBehaviour
{   
    private Vector2 velocity = Vector2.zero;
    private GameObject explodePrefab;

    [SerializeField]
    float projectileSpeed = 3.0F;

    private float timeAlive = 0;

    private void Start()
    {
        if (explodePrefab == null)
        {
            explodePrefab = Resources.Load<GameObject>("Projectiles/GlitterBomb/GlitterExplode");
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Vector2 enemy = enemies[Random.Range(0, enemies.Length)].transform.position;
        Vector2 dir = new Vector2(enemy.x - Player.Instance.transform.position.x, enemy.y - Player.Instance.transform.position.y);

        velocity = dir.normalized * projectileSpeed;
    }

    void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
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