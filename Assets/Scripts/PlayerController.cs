using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerController Instance;

    private float projectileFireRate = 0.5f; // Default projectile fire rate

    private void Awake()
    {
        Instance = this;
    }

    public void SetProjectileFireRate(float newRate)
    {
        projectileFireRate = newRate;
    }

}
