using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LovelyLullaby : ProjectileWeapon {
    public override void ApplyLevelUp(WeaponLevelUp levelUp) {
        base.ApplyLevelUp(levelUp);

        SoundManager.Instance.PlaySoundGlobal(shootSoundName);
    }
}
