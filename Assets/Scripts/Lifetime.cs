using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {
   [Tooltip("How long this game object will life for.")]
   public float lifetime = 1f;

    void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }
}
