using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileJ : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float damage = 20f;

    [HideInInspector]
    public Vector3 direction;

    CircleCollider2D circleCollider;
    Collider2D[] hits = new Collider2D[10];

    private void Awake()
    {
        circleCollider = GetComponentInChildren<CircleCollider2D>();
    }

    private void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        int hitNum = Physics2D.OverlapCircleNonAlloc(transform.position, circleCollider.radius, hits);
        if (hitNum > 0)
        {
            for (int i = 0; i < hitNum; i++)
            {
                if (hits[i].gameObject.CompareTag("Player"))
                {
                    PlayerHealth playerHealth = hits[i].GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.decreaseHealth(damage);
                        Destroy(gameObject);
                    }
                }
            }
        }

    }
}
