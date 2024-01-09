using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleSceenButtons : MonoBehaviour
{
    public string firstLevelSceneName; // update with level scene name once we have one
    public string savedLevelSceneName; // create function that tracks the level the player is on
    public GameObject OptionsUI;       // saves options ui prefab to toggle on/off, this script toggles ON

    [SerializeField] Sprite galaxyButton;

    public void Start()
    {
        var levelNum = SaveManager.SaveData.NextLevel;
        var saveValid = levelNum <= 7 && levelNum >= 2; // precondition for level preview menu

        var continueButton = transform.Find("ContinueButton").GetComponent<RectTransform>();
        var newRunButton = transform.Find("NewRunButton").GetComponent<RectTransform>();
        if (continueButton == null || newRunButton == null) throw new Exception("Title screen is missing some buttons! requires 'ContinueButton' and 'NewRunButton'");

        if (saveValid)
        {
            continueButton.sizeDelta *= 1.3f;
        }
        else
        {
            continueButton.gameObject.SetActive(false);

            newRunButton.sizeDelta *= 1.3f;
            newRunButton.GetComponent<Image>().sprite = galaxyButton;
            newRunButton.GetComponentInChildren<TMP_Text>().fontSize = 24;
        }
    }

    public void ContinueButton()
    {
        // retrieve player data
        SceneManager.LoadScene(savedLevelSceneName);
        //Debug.Log("continue");
    }

    public void NewRunButton()
    {
        SaveManager.ClearSaveData();
        SceneManager.LoadScene(firstLevelSceneName); 
        //Debug.Log("newrun");
    }

    public void OptionsButton()
    {
        // load options
        OptionsUI.SetActive(true); 
    }

    public void QuitButton()
    {
        // quits game if not in platymode
        Application.Quit();
    }

}
