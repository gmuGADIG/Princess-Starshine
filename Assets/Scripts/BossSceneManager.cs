using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runs stuff at the start of boss scenes.
/// </summary>
public class BossSceneManager : MonoBehaviour
{
    public static bool InBossScene = false;

    // There is a boss, but the health has been disabled.
    public static bool InPreDialogue { 
        get => !FindObjectOfType<BossHealth>()?.enabled ?? false;
    }

    // There is a boss and the health has been enabled.
    public static bool InBossFight {
        get => FindObjectOfType<BossHealth>()?.enabled ?? false;
    }

    // There is no boss and we're in a boss scene.
    // WARN: if boss death animations are implemented, this function should be double checked.
    public static bool InPostDialogue {
        get => FindObjectOfType<BossHealth>() == null && InBossScene;
    }

    void Awake() { InBossScene = true; }
    void OnDestroy() { InBossScene = false; }

    void Start()
    {
    }

    public void AfterPreBossDialogue() {
        EquipmentManager.instance.Thaw();
        InGameUI.UpdateItems();

        // TODO: Persist consumables and update consumables UI
    }

    public void ClearSave() {
        SaveManager.ClearSaveData();
    }
}
