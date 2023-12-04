using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LaserWeapon : Weapon 
{
    /** This is for how far the laser travels */
    [SerializeField] private float defDistanceRay = 100;
    /** This is where the laser is fired, aka. the player's position */
    public Transform laserFirePoint;
    /** The laser is represented by a line renderer */
    public LineRenderer m_lineRenderer;
    /** This is where the laser ends, which would be off screen */
    private static Vector2 endVec;
    /** Amount of knockback to inflict on enemies hit by the projectiles. */
    [SerializeField] protected float knockback;
    /** The amount of damage each projectile does. Exact details are left to the projectile script. */
    [SerializeField] protected float damage;
    /** The amount of enemies the projectile can pierce through. 0 means destroy on first hit. -1 means infinite pierce. */
    [SerializeField] protected int pierceCount;
    /** The prefab for the laser */
    [SerializeField] protected Material laserMaterial;

    public void Blast()
    {
        var laser = LineRenderer.Instantiate(m_lineRenderer).GetComponent<Laser>();
        laser.Setup(damage, pierceCount, knockback);
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }

    public override void Update()
    {
        endVec = laserFirePoint.position * defDistanceRay;
        Draw2DRay(laserFirePoint.position, endVec);
    }

    public override void OnEquip() { }
    public override void OnUnEquip() { }
    public override void ProcessOther(Equipment other) { }
    public override void ProcessOtherRemoval(Equipment other) { }
    public override void ApplyLevelUp(WeaponLevelUp levelUp)
    {
        switch (levelUp.type)
        {
            case WeaponLevelUpType.Damage:
                damage += levelUp.amount;
                break;
            case WeaponLevelUpType.KnockBack:
                knockback += levelUp.amount;
                break;
            case WeaponLevelUpType.Pierce:
                pierceCount += (int)levelUp.amount;
                break;
            default:
                throw new Exception("Invalid weapon level-up type!");
        }
    }
}
