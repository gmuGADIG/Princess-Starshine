using UnityEngine;

class FeatherProjectile : Projectile {
    protected override void Move() {
        //
        // 10m/s
        // rev / s = 10 / pi * 2 * r
        // rads / s = revs / s * pi * 2
        // rads / s = 10 / (pi * 2 * r) * pi * 2
        // rads / s = 10 / r
        //
        // Get angular velocity from projectile speed (speed is in m/s)
        var radius = transform.localPosition.magnitude;
        var omega = speed / radius; // rads / s

        var angle = Mathf.Deg2Rad * (Vector3.SignedAngle(transform.localPosition, Vector3.right, Vector3.back));
        var newAngle = angle + omega * Time.deltaTime;

        transform.localPosition = 
            new Vector3(Mathf.Cos(newAngle), Mathf.Sin(newAngle)) * radius;
    }
}
