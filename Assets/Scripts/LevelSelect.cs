using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public static GameObject PauseMenu;
    public static GameObject LevelSelectMenu;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu = Environment.getPauseMenu();
        //LevelSelectMenu = Environment.getLevelSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToPause()
    {

    }
}
