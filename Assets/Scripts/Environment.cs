using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject PauseMenuPrefab;
    public GameObject OptionsMenuPrefab;
    private static GameObject PauseMenu;
    private static GameObject OptionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu = Instantiate(PauseMenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        PauseMenu.SetActive(false);
        OptionsMenu = Instantiate(OptionsMenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
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

    public static GameObject getPauseMenu()
    {
        return PauseMenu;
    }

    public static GameObject getOptionsMenu()
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
