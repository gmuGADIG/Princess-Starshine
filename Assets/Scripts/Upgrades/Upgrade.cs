using System;
using UnityEngine;

public interface IUpgrade {
    public UpgradeType Type();
    public void OnEquip();
    public void OnEquipOther(IUpgrade upgrade);
}