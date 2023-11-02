using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float damage = 20f;

    [HideInInspector]
    public Vector3 direction;

    private void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth playerHealth = GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.decreaseHealth(damage);
        }
    }
}
