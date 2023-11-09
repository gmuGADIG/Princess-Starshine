using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovementAggressive : MonoBehaviour, IBossMovement
{
    public Transform moveTowards;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if(moveTowards == null)
        {
            moveTowards = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(moveTowards != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTowards.position, speed * Time.deltaTime);
        }
    }
}
