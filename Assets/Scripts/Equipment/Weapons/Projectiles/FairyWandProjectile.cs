using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class FairyWandProjectile : MonoBehaviour
{
    //speed of the projectile
    public float speed = 4f;

    //the damage of the projectile
    public float damage = 5f;

    //how long the projectile lasts
    public float projectileLife = 8f;

    private void Start()
    {
        //destroys the projectile after a set time
        Destroy(gameObject, projectileLife);
    }

    void Update()
    {
        //moves the projectile forward
        gameObject.transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //destroys projectile on contact and decrement enemy health
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            if (collision.GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
                enemy.decrementHealth(damage);
            }
        }
    }

}
