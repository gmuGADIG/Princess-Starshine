using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   

    // accleeration is by default set to 80, maxSpeed is set to 10, and deceleration is set to 30
    Vector2 velocity = Vector2.zero;
    [SerializeField]
    float acceleration;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float deceleration;

    //for xp mechanic 
    //Serialization to be tested by user
    [SerializeField]
    int xpPoints = 0;
    //initially serialized for display purposes only
    int xpLevel = 0;

    // Start is called before the first frame update
    void Start() 
    {   
        //Test the xp system with 10 xpPoints
        addXP(10);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (input!=Vector2.zero) {
            velocity += input * acceleration * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        }
        else
        {
            velocity = Vector2.MoveTowards(velocity,Vector2.zero,deceleration*Time.deltaTime);
        }
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    //current placeholder for xp function
    void addXP(int points) 
    {
        xpPoints += points;

        //placeholder for leveling up
        xpLevel = (int)Mathf.Sqrt(xpPoints);
        
    }
}
