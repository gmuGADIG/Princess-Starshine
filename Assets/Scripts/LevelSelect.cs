using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public static GameObject PauseMenu;
    public static GameObject LevelSelectMenu;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu = Environment.getPauseMenu();
        LevelSelectMenu = Environment.getLevelSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToPause()
    {
        PauseMenu.SetActive(true);
        LevelSelectMenu.SetActive(false);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Justin Upgrades");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("TestSceneForSpawner");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Options Menu");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Super Demo Scene");
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("Team 1 Testing");
    }

    public void LoadLevel6()
    {
        SceneManager.LoadScene("tea_time_weapon");
    }
}
