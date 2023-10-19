using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRandoMovement : EnemyTemplate
{

    protected override void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveTowardsObject.transform.position, movementSpeed * Time.deltaTime);
        if (isDead)
        {
            Die();
        }
    }
}
