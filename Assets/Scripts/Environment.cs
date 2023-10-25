using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseGame();
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        Instantiate(PauseMenu, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void unPause()
    {
        Time.timeScale = 1f;
        Debug.Log("Unpause");
        Destroy(PauseMenu);
    }
}
