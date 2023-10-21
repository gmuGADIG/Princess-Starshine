using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class OptionMenu : MonoBehaviour
{
    public static GameObject PauseMenu; // saves pause menu ui object to toggle on/off
    public static GameObject OptionsMenu; // saves options ui object to toggle on/off

    public UnityEvent OnOptionsMenuClose;

    void Start()
    {
        PauseMenu = Environment.getPauseMenu();
        OptionsMenu = Environment.getOptionsMenu();
    }

    public void goToPause()
    {
        OnOptionsMenuClose.Invoke();

        // if this code causes a null exception in the level preview menu, it wasnt my fault :P
        if (PauseMenu != null && OptionsMenu != null) {
            PauseMenu.SetActive(true);
            OptionsMenu.SetActive(false);
        }
    }
}
