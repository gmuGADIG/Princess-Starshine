using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : IBossMovement
{

    private float movingTimer = 2;
    public void Move(float speed)
    {

        movingTimer -= Time.deltaTime;
        if (movingTimer <= 0)
        {


            GameObject player = GameObject.FindGameObjectWithTag("Player");

            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveTowardsObject.transform.position, movementSpeed * Time.deltaTime);
            movingTimer = 2;
        }
    }
}
