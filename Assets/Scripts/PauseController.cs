using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static GameObject PauseMenu;
    public static GameObject OptionsMenu;
    public static GameObject LevelSelect;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu = Environment.getPauseMenu();
        OptionsMenu = Environment.getOptionsMenu();
        LevelSelect = Environment.getLevelSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void unPause()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }

    public void goToOptions()
    {
        OptionsMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void goToLevelSelect()
    {
        LevelSelect.SetActive(true);
        PauseMenu.SetActive(false);
    }
}
