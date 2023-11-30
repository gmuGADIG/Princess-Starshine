using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalrogCoinWeapon : MonoBehaviour
{
    //The spread of the weapon in degrees so 45 degrees from the 0 this means the spread is +45 to -45
    public float spread;
    public float damage;
    public int pierceCount;
    public float speed;
    public float knockback;
    public float size;
    public float shotTime;
    public float projectileAmount;
    private float projectileCount;
    private float shotTimer;
    private bool enable = true;

    public GameObject projectile;
    public GameObject player;


    public void Start()
    {
    }

    public void Update()
    {
        if (enable)
        {
            if (shotTime < shotTimer)
            {
                shotTimer = 0;
                if (projectileAmount > projectileCount)
                {
                    fire(player.transform.position);
                    projectileCount++;
                }
                else
                {
                    enable = false;
                }
            }
            else
            {
                shotTimer += Time.deltaTime;
            }
        }
    }

    public void fire(Vector3 target)
    {
        Vector3 currentPos = transform.position;
        Vector2 triangle = currentPos - target;
        float angle = Mathf.Atan2(-triangle.y, -triangle.x);
        float fireAngle = Random.Range(angle-(Mathf.Deg2Rad*spread), angle+(Mathf.Deg2Rad*spread));
        Vector2 velocity = new Vector2(Mathf.Cos(fireAngle), Mathf.Sin(fireAngle));

        GameObject proj = Instantiate(projectile);
        proj.transform.position = currentPos;
        BalrogCoinProjectile coinProjectile;
        proj.TryGetComponent<BalrogCoinProjectile>(out coinProjectile);
        if (coinProjectile)
        {
            coinProjectile.Setup(velocity, damage, pierceCount, speed, knockback, size);
        }
    }

    public void startAttack()
    {
        projectileCount = 0;
        enable = true;
    }

}
