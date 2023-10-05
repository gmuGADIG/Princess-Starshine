using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        //OptionsMenu = Environment.getOptionsMenu();
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
}
