using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMachineGun : BossWeaponJ
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float attackLength = 2f;
    [SerializeField] float attackInterval = 3f;
    [SerializeField] float projectilesPerMinute = 100;

    float projectileDelay;

    Transform playerTransform;

    void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        projectileDelay = 60 / projectilesPerMinute;
        StartCoroutine(DelayCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        float timer = 0f;
        while (timer < attackLength)
        {

            BossProjectileJ proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<BossProjectileJ>();
            if (proj != null)
            {
                proj.direction = -(transform.position - playerTransform.position).normalized;
            }
            yield return new WaitForSeconds(projectileDelay);
            timer += projectileDelay;
        }
        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(attackInterval);
        StartCoroutine(ShootCoroutine());
    }

}
