using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public static GameObject PauseMenu; // saves pause menu ui object to toggle on/off
    public static GameObject OptionsMenu; // saves options ui object to toggle on/off

    void Start()
    {
        PauseMenu = Environment.getPauseMenu();
        OptionsMenu = Environment.getOptionsMenu();
    }

    public void goToPause()
    {
        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }
}
