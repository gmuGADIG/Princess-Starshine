using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPVacuum : MonoBehaviour
{
    [SerializeField] LayerMask xpLayer;
    [SerializeField] float radius = 2f;
    [SerializeField] float pullMax = 3f;

    Collider2D[] results = new Collider2D[50];
    float sqrRadius;

    private void Awake()
    {
        sqrRadius = radius * radius;
    }

    private void Update()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, results, xpLayer);
        for (int i = 0; i < hitCount; i++)
        {
            float sqrDistance = (transform.position - results[i].transform.position).sqrMagnitude;
            Vector3 direction = -(results[i].transform.position - transform.position).normalized;
            float velocity = (1 - (sqrDistance / sqrRadius)) * pullMax;
            results[i].transform.Translate(direction * velocity * Time.deltaTime);
        }
    }
}
