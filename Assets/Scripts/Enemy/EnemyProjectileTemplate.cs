using System;
using UnityEngine;

/*
Basic EnemyProjectile class. 
*/

public class Projectile : MonoBehaviour
{

    []
    //After this many seconds, projectiles will be automatically removed
    [SerializeField] 
    private float maxLifeTime = 100;

    // How fast the object moves
    [SerializeField] 
    private int velocity;

    // How much damage the projectile will do
    [SerializeField] 
    private int damage = 0;

    //how big the object is
    [SerializeField] 
    private int size = 1;

    //how long the projectile has been alive
    private int timeAlive = 0;


    protected virtual void Update()
    {
        transform.position += (Vector3) velocity * Time.deltaTime;

        timeAlive += Time.deltaTime;
        if (timeAlive > maxLifeTime)
        {
            Destroy(this.gameObject);
        }
    }

}
