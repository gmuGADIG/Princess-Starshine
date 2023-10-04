using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int tempHealth;
    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        tempHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempHealth > 100)
            tempHealth = 100;
        else if (tempHealth < 0)
        {
            tempHealth = 0;
            isDead = true;
        }
    }

    public void decreaseHealth(int num)
    {
        tempHealth -= num;
    }

    public void increaseHealth(int num)
    {
        tempHealth += num;
    }
}
