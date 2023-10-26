using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterExplode : MonoBehaviour
{
    float damage;

    void Start()
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(this.gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
        this.GetComponent<ProjectileCollision>().SetDamage(newDamage);
    }
}