using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpOrb : MonoBehaviour
{   
    public int points = 1;
    [Tooltip("The multiplier of the XP points collected when in the wall of fire.")]
    [SerializeField] float fireMultiplier = 0.5f;

    void Start() {
        GetComponent<CircleCollider2D>().radius = ConsumableManager.Instance.XpCollisionRadius.Value;
    }

    void Update() {
        if (transform.position.x < TeaTime.cameraBoundingBox().xMin + 1) {
            Destroy(gameObject);
            Player.instance.AddXP(points * fireMultiplier, false);
        }
    }
}
