using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float tempHealth;
    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        tempHealth = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempHealth > maxHealth)
            tempHealth = maxHealth;
        else if (tempHealth <= 0)
        {
            tempHealth = 0;
            isDead = true;
        }
    }

    public void decreaseHealth(float num)
    {
        tempHealth -= num;
    }

    public void increaseHealth(float num)
    {
        tempHealth += num;
        Debug.Log(tempHealth);
    }
}
