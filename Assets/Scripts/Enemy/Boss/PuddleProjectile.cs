using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleProjectile : BossProjectile
{
    private float sizeChange;
    private float damageDelay;
    private float damageDelayTimer;
    private bool changeSize;
    private CircleCollider2D circleCollider;


    private void Start()
    {
        //TryGetComponent<CircleCollider2D>(out circleCollider);
        circleCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeSize)
        {
            transform.localScale += transform.localScale * sizeChange * Time.deltaTime;
        }
        if (damageDelayTimer >= damageDelay)
        {
            circleCollider.enabled = true;
            changeSize = true;
            //can also put animation things here
        }
        if (aliveTimer > maxAliveTime)
        {
            Destroy(gameObject);
        }
        aliveTimer += Time.deltaTime;
        damageDelayTimer += Time.deltaTime;
    }

    public void Setup(float damage, float maxAliveTime, float sizeChange, float damageDelay, bool changeSizeDelay)
    {
        this.damage = damage;
        this.maxAliveTime = maxAliveTime;
        this.damageDelay = damageDelay;
        this.sizeChange = sizeChange;
        circleCollider = GetComponentInChildren<CircleCollider2D>();

        if (damageDelay == 0)
        {
            circleCollider.enabled = true;
        }
        changeSize = !changeSizeDelay;
    }
}
