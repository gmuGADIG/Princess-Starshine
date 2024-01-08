using UnityEngine;

public class PrettyParasol : ProjectileWeapon {
    PrettyParasolSprite sprite;

    float FireRateModifier { get => (1 + statModifiers.fireRate) * (1 + staticStatModifiers.fireRate); }

    [SerializeField] GameObject spritePrefab;

    void Setup() {
        Debug.Assert(Instantiate(spritePrefab, Player.instance.transform).TryGetComponent(out sprite));

        // we're only going to fire when the correct frame in the animation is displayed
        sprite.ShouldFire += base.Fire; 
    }

    public override void OnEquip() {
        base.OnEquip();
        Setup();
    }

    protected override void Thaw(object data) {
        base.Thaw(data);
        Setup();
    }

    protected override void Fire()
    {
        sprite.animator.speed = FireRateModifier;
        // invokes the animation, which will invoke sprite.ShouldFire which will call base.Fire
        sprite.Swinging = true; 
    }
}
