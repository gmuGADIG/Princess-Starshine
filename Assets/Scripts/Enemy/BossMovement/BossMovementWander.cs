using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossMovementWander : MonoBehaviour
{
    public float speed = 1f;
    public float waitTime = 3f;
    public float detectionRadius = 5f;
    private float timer;
    public Transform moveTo;
    
    // Start is called before the first frame update
    void Start()
    {
        SelectMoveTo();
        timer = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < waitTime)
        {
               timer += Time.deltaTime;
        } else
        {
            timer = 0;
            SelectMoveTo();
        }
        if(moveTo != null)
        {
            Vector2 direction = (moveTo.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }
    private void SelectMoveTo()
    {
        Vector2 midpoint = GetMidpoint();
        //This is also probably bad and inefficient but its also the simplest way. remind me to optimize enemies if we have time.
        RaycastHit2D[] objects = Physics2D.CircleCastAll(midpoint, detectionRadius, Vector2.zero);
        objects = objects.Where(x => x.collider.gameObject?.tag.Contains("Enemy") ?? false).ToArray();
        int select = Random.Range(0, objects.Length+1);
        if(select == objects.Length)
        {
            moveTo = Player.instance?.transform;
        } else
        {
            moveTo = objects[select].collider.transform;
        }
    }
    private Vector2 GetMidpoint()
    {
        if(Player.instance == null)
        {
            return transform.position;
        } else
        {
            return (Player.instance.transform.position + transform.position) / 2;
        }
    }
}
