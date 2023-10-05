using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterPrimary : MonoBehaviour
{
    Vector2 velocity = Vector2.zero;

    void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    public void GetFired(float angle, float speed)
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        velocity = (new Vector2(x,y))*speed;
    }
}