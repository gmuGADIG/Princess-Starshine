using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class OptionMenu : MonoBehaviour {
    public GameObject PauseMenu; // saves pause menu ui object to toggle on/off
    public GameObject OptionsMenu; // saves options ui object to toggle on/off

    public UnityEvent OnOptionsMenuClose;

    void Start() {
        // We only want to get these variables if Options was opened by pausing the game.
        // For the Main Menu it's linked up, but without the Pause Menu.
        if (Environment.getPauseMenu() != null) {
            PauseMenu = Environment.getPauseMenu();
            OptionsMenu = Environment.getOptionsMenu();
        }
    }

    public void goToPause() {
        OnOptionsMenuClose.Invoke();

        // if this code causes a null exception in the level preview menu, it wasnt my fault :P
        if (PauseMenu != null && OptionsMenu != null) {
            PauseMenu.SetActive(true);
            OptionsMenu.SetActive(false);
        }
    }

    public void BackButton() {
        OptionsMenu.SetActive(false);
    }
}
