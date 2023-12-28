using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runs stuff at the start of boss scenes.
/// </summary>
public class BossSceneManager : MonoBehaviour
{
    void Start()
    {
        EquipmentManager.instance.Thaw();
        EquipmentManager.instance.gameObject.SetActive(false);
        InGameUI.UpdateItems();

        // TODO: Persist consumables and update consumables UI
    }
}
