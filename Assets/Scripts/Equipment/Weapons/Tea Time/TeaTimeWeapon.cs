using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaTimeWeapon : MonoBehaviour
{
    //If any of these variable names need changed feel free to change them to better match others

    public int maxProj;
    //Number of projectiles
    public int numberProj;
    //The teatime projectile prefab
    public GameObject projectile;
    //An Array of directions that will be randomly chosen when firing a projectile
    public Vector2[] directions;

    // Update is called once per frame
    void Update()
    {
        fireWeapon();
    }

    void lostProjectile()
    {
        numberProj--;
        fireWeapon();
    }

    void fireWeapon()
    {
        if (maxProj <= numberProj)
        {
            return;
        }
        numberProj++;
        GameObject temp = Instantiate(projectile, gameObject.transform);
        Vector3 dir = directions[(int)(Random.Range(0, directions.Length))];
        temp.GetComponent<TeaTime>().dir = dir;

    }
}
