using UnityEngine;

public class BubbleProjectile : Projectile {
    new SpriteRenderer renderer;

    protected override void Start() {
        base.Start();

        var sizeDelta = .2f;
        var rand = Random.Range(-sizeDelta, sizeDelta);
        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.gameObject.transform.localScale *= 1 + rand;
    }

    protected override void Update(){
        base.Update();

        if (!renderer.isVisible) {
            Destroy(gameObject);
        }
    }
}
