using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitProjectile : BossProjectile
{
    private Vector3 acceleration;
    private float centripetalAcceleration;
    private Vector3 velocityUnitVector;
    private float periodChange;
    private float distanceChange;
    private float radius;
    private float period;
    private float timeUntilChange;
    private float changeTimer;
    private bool doChange;
    private float finalPeriod;
    private bool finalLarger;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        velocity += acceleration * Time.fixedDeltaTime;
        velocityUnitVector = velocity.normalized;
        acceleration.x = velocityUnitVector.y;
        acceleration.y = -velocityUnitVector.x;
        acceleration *= centripetalAcceleration;
        velocity = velocityUnitVector * speed;

        speed = ((2 * Mathf.PI * radius) / period);
        centripetalAcceleration = (Mathf.Pow(speed, 2) / radius);

        if (timeUntilChange <= changeTimer)
        {
            radius += distanceChange;
            if (finalLarger && finalPeriod <= period)
            {
                period = finalPeriod;
            }
            else if (finalPeriod >= period)
            {
                period = finalPeriod;
            }
            else
            {
                period += periodChange;
            }

            
        }
        if (doChange)
        {
            changeTimer += Time.fixedDeltaTime;
        }
        if (aliveTimer >= maxAliveTime)
        {
            Destroy(gameObject);
        }
        aliveTimer += Time.fixedDeltaTime;
    }
    public override void Update()
    {

    }

    public void Setup(float damage, float period, float maxAliveTime, float startingAngle, float radius, float periodChange, float finalPeriod, float distanceChange, float timeTillChange, bool doChange)
    {
        this.damage = damage;
        this.maxAliveTime = maxAliveTime;
        this.periodChange = periodChange;
        this.distanceChange = distanceChange;
        this.radius = radius;
        this.period = period;
        this.finalPeriod = finalPeriod;
        finalLarger = (finalPeriod >= period);
        this.doChange = doChange;
        this.timeUntilChange = timeTillChange;
        speed = ((2 * Mathf.PI * radius) / period);
        velocityUnitVector = new Vector2(Mathf.Sin(startingAngle), -Mathf.Cos(startingAngle));
        velocity = velocityUnitVector * speed;
        centripetalAcceleration = (Mathf.Pow(speed, 2) / radius);
        acceleration = new Vector2(velocityUnitVector.y, -velocityUnitVector.x) * centripetalAcceleration;
    }
}
