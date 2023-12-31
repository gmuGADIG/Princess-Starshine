using UnityEngine;
using Newtonsoft.Json;
using System.IO;
//using UnityEditor;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public static SaveData SaveData = new();

    string savename = "save";

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        } 

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Update() {
        if (Application.isEditor && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.M))
            NewGame();
    }
    
    public void NewGame() {
        PlayerPrefs.DeleteKey(savename);
        SaveData = new();
    }

    public void Load() {
        if (PlayerPrefs.HasKey(savename)) {
            var save = PlayerPrefs.GetString(savename, null);
            SaveData = JsonConvert.DeserializeObject<SaveData>(save);
        }
    }

    public void Save() {
        var json = JsonConvert.SerializeObject(SaveData);
        PlayerPrefs.SetString(savename, json);
    }

    void OnDestroy() {
        if (Instance == this) {
            Save();
        }
    }

    //[MenuItem("SaveTools/ClearSaveData")]
    //static void ClearSaveData()
    //{
    //    PlayerPrefs.DeleteAll();
    //}
}
