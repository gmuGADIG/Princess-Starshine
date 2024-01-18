using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPDebug : MonoBehaviour {
    [Tooltip("Sets <b>SaveManager.SaveData.NextLevel</b> to nextLevel if the game is running in editor and <b>SaveManager.SaveData.NextLevel</b> is set to 0 (its default value).")]
    [SerializeField] int nextLevel;

    void Awake() {
        if (SaveManager.SaveData.NextLevel == 0 && Application.isEditor) {
            SaveManager.SaveData.NextLevel = nextLevel;
        }
    }
}
