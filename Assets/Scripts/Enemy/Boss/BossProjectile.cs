using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    //The base Projectile is for machine gun
    protected float damage;
    protected Vector3 velocity;
    protected float speed;
    protected float maxAliveTime = 10;
    protected float aliveTimer;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        transform.position = transform.position + (velocity * Time.deltaTime * speed);
        if(aliveTimer > maxAliveTime)
        {
            Destroy(gameObject);
        }
        aliveTimer += Time.deltaTime;
    }

    public virtual void Setup(Vector2 velocity, float damage, float speed, float maxAliveTime)
    {
        this.velocity = velocity;
        this.speed = speed;
        this.damage = damage;
        this.maxAliveTime = maxAliveTime;
    }
}
