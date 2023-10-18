using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FairyWandWeapon : MonoBehaviour
{
    //cooldown rate of firing
    public float cooldownRate = 0.25f;

    //current cooldown timer
    private float cooldown;

    //rotation of the projectile
    private Vector3 rotationPosition;

    //the nearest enemy
    private GameObject nearestEnemy;

    //the projectile being fired
    public GameObject projectile;

    void Start()
    {
        //starts the current cooldown timer
        cooldown = cooldownRate;
    }

    void Update()
    {
        //array of enemies with the tag "Enemy"
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

        //calculates the nearest enemy
        float closest = Mathf.Infinity;
        foreach (GameObject e in enemy)
        {
            Vector3 direction = e.transform.position - transform.position;
            float target = direction.sqrMagnitude;
            if (target < closest)
            {
                closest = target;
                nearestEnemy = e;
            }
        }

        //fire projectiles if there are enemy present
        if (enemy.Length > 0)
            {
                rotationPosition = nearestEnemy.transform.position - transform.position;
                cooldown -= Time.deltaTime;
            if (cooldown < 0)
                {
                    Instantiate(projectile, gameObject.transform.position, Quaternion.LookRotation(Vector3.forward, rotationPosition));
                    cooldown = cooldownRate;
                }
            }
            

    }

}
