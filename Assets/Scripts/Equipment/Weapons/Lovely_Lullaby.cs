using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LovelyLullaby : MonoBehaviour
{
    CircleCollider2D circle;
    // Start is called before the first frame update
    public void Start()
    {
        circle = GetComponent<CircleCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (circle != null)
        {

        }

    }
    void OnTriggerEnter2D(Collider2D collider)
    {

        OnTriggerExit2D(collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log(collider);
        EnemyTemplate enemy = collider.GetComponent<EnemyTemplate>();
        if (enemy != null)
        {
            enemy.SetMovementSpeed((short)-enemy.GetMovementSpeed());
        }

    }
}
