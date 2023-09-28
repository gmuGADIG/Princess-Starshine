using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public GameObject OptionsUI; // saves options ui prefab to toggle on/off, this script toggles OFF
    //public GameObject PauseMenu;
    public void BackButton()
    {
        //Instantiate(PauseMenu, new Vector3(0, 0, 0), Quaternion.identity);
        //Destroy(this);

        OptionsUI.SetActive(false);
    }
}
