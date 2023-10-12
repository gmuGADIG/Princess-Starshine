using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private float movementSpeed = 10f;

    // Update is called once per frame
    void Update() 
    {
        float HorizontalMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float VerticalMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        gameObject.transform.Translate(HorizontalMovement, VerticalMovement, 0);
    }
}
