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
