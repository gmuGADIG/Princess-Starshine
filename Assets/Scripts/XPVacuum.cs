using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPVacuum : MonoBehaviour
{
    [SerializeField] LayerMask xpLayer;
    [SerializeField] float radius = 2f;
    [SerializeField] float pullMax = 3f;

    Collider2D[] xpInRange = new Collider2D[50];
    float sqrRadius;

    private void Awake()
    {
        sqrRadius = radius * radius;
    }

    private void Update()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, xpInRange, xpLayer);
        for (int i = 0; i < hitCount; i++)
        {
            float distance = (transform.position - xpInRange[i].transform.position).magnitude;
            Vector3 direction = -(xpInRange[i].transform.position - transform.position).normalized;
            float velocity = (1 - (distance / radius)) * pullMax;
            xpInRange[i].transform.Translate(direction * velocity * Time.deltaTime);
        }
    }
}
