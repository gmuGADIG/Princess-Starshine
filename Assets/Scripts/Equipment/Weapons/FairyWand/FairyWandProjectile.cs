using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class FairyWandProjectile : Projectile
{
    private float projectileLife = 5f;

    private void Start()
    {
        Destroy(gameObject, projectileLife);
    }

}
