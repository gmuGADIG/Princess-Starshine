using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterExplode : MonoBehaviour
{
    [HideInInspector] public float damage = 10;

    void Update()
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.BroadcastMessage("Damage", 5.0,SendMessageOptions.DontRequireReceiver);
        }
    }
}