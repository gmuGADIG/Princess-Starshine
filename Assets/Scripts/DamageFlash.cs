using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Flashes a sprite red for a short amount of time. Does not trigger itself; another script must do that when it detects a hit. 
 */
public class DamageFlash : MonoBehaviour
{
    public Color DamagedColor = Color.red;
    public Color HealthyColor = Color.white;

    public SpriteRenderer sprite;
    const float FlashDuration = 0.1f;
    float flashTimeRemaining;

    void Update()
    {
        if (flashTimeRemaining <= 0) sprite.color = HealthyColor;
        else sprite.color = DamagedColor;

        flashTimeRemaining -= Time.deltaTime;
    }

    public void Damage()
    {
        flashTimeRemaining = FlashDuration;
    }
}
