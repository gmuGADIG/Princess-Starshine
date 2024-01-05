using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Tooltip("How big the explosion starts as.")]
    [SerializeField] float initialScale = 1f;

    [Tooltip("How long the explosion lasts for.")]
    [SerializeField] float lifetime = 2f;

    /// <summary>
    /// The initial lifetime of the explosion.
    /// </summary>
    float initialLifetime;
    void Start() {
        initialLifetime = lifetime;
    }

    void Update() {
        lifetime -= Time.deltaTime;
        transform.localScale = Vector3.one * Mathf.Lerp(initialScale, 0f, 1 - lifetime / initialLifetime);

        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }
}
