using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenEquipment {
    public string Type;
    public int LevelUpsDone;
    public object Data;
}

public class SaveData  {
    public int NextLevel;
    public List<FrozenEquipment> frozenEquipment = new();
    public int PlayerLevel = 1;
    public float PlayerXP = 0;
    public Consumable.Type HeldConsumable = Consumable.Type.None;
}
