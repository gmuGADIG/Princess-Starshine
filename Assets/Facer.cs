using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component to flip a sprite based on whether or not the sprite is moving left or right.
/// </summary>
public class Facer : MonoBehaviour
{
    float prevX = 0f;

    SpriteRenderer sprite;
    void Start() {
        Debug.Assert(TryGetComponent(out sprite));
    }

    void Update() {
        var velX = transform.position.x - prevX;

        sprite.flipX = velX < 0;

        prevX = transform.position.x;
    }
}
