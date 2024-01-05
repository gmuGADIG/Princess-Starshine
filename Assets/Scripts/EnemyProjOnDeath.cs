using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjOnDeath : MonoBehaviour
{
    [Tooltip("The projectile that the enemy shoots.")]
    public GameObject bullet;

    [Tooltip("The speed the bullet travels at.")]
    public float bulletSpeed = 5f;

    [Tooltip("How long the bullet lives for.")]
    public float bulletLifetime = 5f;

    [Tooltip("How much damage the bullet will do.")]
    public float bulletDamage = 2f;

    void OnDisable()
    {
        if(!gameObject.scene.isLoaded) return; // Prevents instantiating bullet when scene isn't loaded
        var projectile = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<EnemyBullet>();

        var toPlayerHat = ((Vector2)(Player.instance.transform.position - transform.position)).normalized;
        projectile.Setup(toPlayerHat * bulletSpeed, bulletLifetime, bulletDamage);
    }

}
