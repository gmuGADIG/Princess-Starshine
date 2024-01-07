using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceenButtons : MonoBehaviour
{
    public string firstLevelSceneName; // update with level scene name once we have one
    public string savedLevelSceneName; // create function that tracks the level the player is on
    public GameObject OptionsUI;       // saves options ui prefab to toggle on/off, this script toggles ON

    public void Start()
    {
        var saveExists = PlayerPrefs.HasKey("save");
        var continueButton = transform.Find("ContinueButton").GetComponent<RectTransform>();
        var newRunButton = transform.Find("NewRunButton").GetComponent<RectTransform>();
        if (continueButton == null || newRunButton == null) throw new Exception("Title screen is missing some buttons! requires 'ContinueButton' and 'NewRunButton'");

        if (saveExists)
        {
            continueButton.sizeDelta *= 1.3f;
        }
        else
        {
            continueButton.gameObject.SetActive(false);
            newRunButton.sizeDelta *= 1.3f;
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
        SaveManager.Instance.NewGame();
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
