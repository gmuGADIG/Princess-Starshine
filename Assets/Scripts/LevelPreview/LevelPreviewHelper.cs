using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreviewHelper : MonoBehaviour
{
    [Tooltip("This is the scene name of the next level. (i.e. the scene name of the level preview node you want to unlock).")]
    public string NextLevelSceneName;

    void OnDestroy() {
        // this should be called when the scene is changing *fingers crossed*
        SaveManager.SaveData.FurthestLevelSceneName = NextLevelSceneName;
    }
}
