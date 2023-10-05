using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu = Instantiate(PauseMenu, new Vector3(0, 0, 0), Quaternion.identity);
        PauseMenu.SetActive(false);
        OptionsMenu = Instantiate(OptionsMenu, new Vector3(0, 0, 0), Quaternion.identity);
        OptionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !PauseMenu.activeSelf)
        {
            pauseGame();
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && PauseMenu.activeSelf)
        {
            unPause();
        }
    }

    public GameObject getPauseMenu()
    {
        return PauseMenu;
    }

    public GameObject getOptionsMenu()
    {
        return OptionsMenu;
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    public void unPause()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }
}
