using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowRay : LaserWeapon
{
    // Start is called before the first frame update
    public RainbowRay()
    {
        /** Amount of knockback to inflict on enemies hit by the projectiles. */
        knockback = 0f;

        /** The amount of damage each projectile does. Exact details are left to the projectile script. */
        damage = 0f;

        /** The amount of enemies the projectile can pierce through. 0 means destroy on first hit. -1 means infinite pierce. */
        pierceCount = -1;

        /** The laser is a line renderer, so it uses a material instead of a game object */
        laserMaterial = Resources.Load<Material>("Rainbow");
    }
}
