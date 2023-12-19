using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreviewHelper : MonoBehaviour
{
    [Tooltip("This is the scene name of the next level. (i.e. the scene name of the level preview node you want to unlock).")]
    public string NextLevelSceneName;

    void BossDied() {
        SaveManager.SaveData.FurthestLevelSceneName = NextLevelSceneName;
    }

    void Start() {
        BossHealth.BossDied += BossDied;
    }

    void OnDestroy() {
        BossHealth.BossDied -= BossDied;
    }
}
