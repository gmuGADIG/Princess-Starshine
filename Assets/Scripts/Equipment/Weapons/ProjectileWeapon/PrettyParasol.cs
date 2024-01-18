using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class PrettyParasol : ProjectileWeapon {
    PrettyParasolSprite sprite;

    float FireRateModifier { get => (1 + statModifiers.fireRate) * (1 + staticStatModifiers.fireRate); }

    [SerializeField] GameObject spritePrefab;

    /// <summary>
    /// Ensures <paramref name="shouldBeTrue"/> is true by throwing an exception otherwise.
    /// <para>This function is not optimized away during release builds, so it should 
    /// be used over <c>Debug.Assert</c> if <paramref name="shouldBeTrue"/> 
    /// is derived from side effects (e.g. <c>Assert(TryGetComponent(out etc))</c>)</para>
    /// </summary>
    /// <param name="shouldBeTrue">If false, this function will throw.</param>
    /// <exception cref="System.Exception">Thrown when <paramref name="shouldBeTrue"/> is false.</exception>
    public static void Assert(bool shouldBeTrue) {
        if (!shouldBeTrue) {
            throw new System.Exception("Assertion failed.");
        }
    }

    void Setup() {
        
        Assert(Instantiate(spritePrefab, Player.instance.transform).TryGetComponent(out sprite));

        // we're only going to fire when the correct frame in the animation is displayed
        sprite.ShouldFire += base.Fire; 
    }

    public override void OnEquip() {
        base.OnEquip();
        Setup();
    }

    public override void OnUnEquip() {
        Destroy(sprite.gameObject);
    }

    protected override void Thaw(object data) {
        base.Thaw(data);
        Setup();
    }

    protected override void Fire() {
        sprite.animator.speed = FireRateModifier;
        // invokes the animation, which will invoke sprite.ShouldFire which will call base.Fire
        sprite.Swinging = true; 
    }
}
