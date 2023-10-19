using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparks : MonoBehaviour
{
    //Falling speed
    [SerializeField] private float gravity = 2f;

    void Start()
    {
        //Destroys the object after 1 second so that it won't be infinitely spawning
        Destroy(gameObject, 1);
    }

    void Update()
    {
        //Makes the spark fall down without needing rigidbody
        transform.Translate(Vector3.down * gravity * Time.deltaTime);
    }
}
