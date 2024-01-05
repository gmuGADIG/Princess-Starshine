using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Vector2 velocity;

    IEnumerator LifetimeHandler(float lifetime) {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    bool hasBeenSetup = false;
    public void Setup(Vector2 velocity, float lifetime, float damage) {
        this.velocity = velocity;

        gameObject.AddComponent<Damage>().damage = damage;

        StartCoroutine(LifetimeHandler(lifetime));

        hasBeenSetup = true;
    }

    void Update() {
        Debug.Assert(hasBeenSetup);

        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
