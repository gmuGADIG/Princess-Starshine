public class FairyWandProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();
        maxLifeTime = 5;
    }
}