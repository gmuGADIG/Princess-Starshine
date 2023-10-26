using UnityEngine;

public class GlitterPrimary : Projectile
{
    static GameObject explosionPrefab;
    
    protected override void Start()
    {
        base.Start();
        maxLifeTime = 1.5f;
        if (explosionPrefab == null) explosionPrefab = Resources.Load<GameObject>("Projectiles/GlitterBomb/GlitterExplode");
    }

    protected override void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return; // prevents an error message when closing the scene (creation objects on destroy)
        base.OnDestroy();
        
        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<GlitterExplode>();
        explosion.SetDamage(damage);
    }
}