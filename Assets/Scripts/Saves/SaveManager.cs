using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public static SaveData SaveData = new SaveData();

    void Awake() {
        if (Instance != null) {
            Debug.LogError("More than one SaveManager exists!");
        } Instance = this;

        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Load() {
        // TODO
    }

    public void Save() {
        // TODO
    }

    void OnDestroy() {
        Debug.LogWarning("SaveManager is being destroyed! Saving game! (This should only happen when the game closes)");
        Save();
    }
}
