using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPVacuum : MonoBehaviour
{
    [SerializeField] LayerMask xpLayer;
    [SerializeField] float radius = 2f;
    [SerializeField] float pullMax = 3f;

    Collider2D[] xpInRange = new Collider2D[50];

    public static XPVacuum Instance { get; private set; }
    public BuffableStat Radius;

    void Awake()
    {
        Instance = this;
        Radius = new(radius);
    }

    private void Update()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, Radius.Value, xpInRange, xpLayer);
        for (int i = 0; i < hitCount; i++)
        {
            float distance = (transform.position - xpInRange[i].transform.position).magnitude;
            Vector3 direction = -(xpInRange[i].transform.position - transform.position).normalized;
            float velocity = (1 - (distance / Radius.Value)) * pullMax;
            xpInRange[i].transform.Translate(direction * velocity * Time.deltaTime);
        }
    }
}
