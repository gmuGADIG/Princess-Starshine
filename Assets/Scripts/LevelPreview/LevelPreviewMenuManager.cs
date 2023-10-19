using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro; // textmesh stuff

public class LevelPreviewMenuManager : MonoBehaviour
{
    public LPNode FirstNode;
    public TMP_Text LevelText;
    public GameObject LevelPreviewUI;
    public GameObject OptionsMenu;

    public static bool OptionsMenuVisible { get; private set; } = false;
    
    public void OnLeftLevel() {
        LevelText.text = "";
    }

    public void OnAtLevel(string levelName) {
        LevelText.text = levelName;
    }

    public void OnSwapToLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void OnOptionButtonClick() {
        OptionsMenuVisible = true;

        OptionsMenu.SetActive(true);
        LevelPreviewUI.SetActive(false);
    }

    public void OnOptionsMenuClose() {
        OptionsMenuVisible = false;

        OptionsMenu.SetActive(false);
        LevelPreviewUI.SetActive(true);
    }

    public void OnMenuButtonClick() {
        SceneManager.LoadScene("TitleScreenScene");
    }

    public void Start() {
        LPPlayer.Instance.CurrentNode = FirstNode;
        LevelText.text = "";

        LPPlayer.LeftLevel += OnLeftLevel;
        LPPlayer.AtLevel += OnAtLevel;
        LPPlayer.SwapToLevel += OnSwapToLevel;
    }
}
