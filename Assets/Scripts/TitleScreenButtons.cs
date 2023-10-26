using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        SceneManager.LoadScene(savedLevelSceneName); // TEMPORARY, NEED LEVELS AND SAVE SYSTEM
        //Debug.Log("continue");
    }

    public void NewRunButton()
    {
        SceneManager.LoadScene(firstLevelSceneName); // TEMPORARY, NEED LEVELS
        //Debug.Log("newrun");
        PlayerPrefs.SetInt("HasLaunched", 1); //Set to 1 after player has played the game
    }

    public void OptionsButton()
    {
        // load options
        OptionsUI.SetActive(true); 
    }

    public void QuitButton()
    {
        if(Application.isPlaying)
        {
            // closes playmode 
            EditorApplication.isPlaying = false;
        }
        else
        {
            // quits game if not in platymode
            Application.Quit();
        }
        
    }

}
