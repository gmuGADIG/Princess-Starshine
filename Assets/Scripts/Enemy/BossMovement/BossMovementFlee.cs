using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovementFlee : IBossMovement
{
    public Transform fleeFrom;
    public float speed = 5f;
    public float strafeRatio = 0.5f;
    public float strafeSpeed = 1f;
    public float strafeTimer = 3f;
    private float strafeTime;
    private bool strafeDir = false;
    // Start is called before the first frame update
    void Start()
    {
        if (fleeFrom == null)
        {
            fleeFrom = GameObject.FindGameObjectWithTag("Player").transform;
        }
        strafeTime = strafeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (fleeFrom != null)
        {
            Vector2 move = new Vector2(0, 0);

            move += (Vector2)transform.position - (Vector2)fleeFrom.position;
            if(strafeTime <= 0)
            {
                strafeDir = !strafeDir;
                strafeTime = strafeTimer;
            }
            else
            {
                strafeTime -= Time.deltaTime;
            }
            move += (Vector2)Vector3.Cross((Vector2)transform.position - (Vector2)fleeFrom.position, Vector3.forward) * (strafeDir ? 1 : -1) * strafeRatio;
            move.Normalize();
            transform.position = (Vector2)transform.position + speed * Time.deltaTime * move;
        }
    }
}
