using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    public Button ContinueButtonUI;
    private ColorBlock originalColor;

    void Awake()
    {
        // PlayerPrefs.DeleteAll(); // UNCOMMENT TO RESET HasLaunched TO 0
        originalColor = ContinueButtonUI.colors;
        //change "Continue" color if no save found
        if (PlayerPrefs.GetInt("HasLaunched", 0) == 0)
        {
            //grey out button color and disable interaction
            ContinueButtonUI.GetComponent<Image>().color = Color.gray;
            ContinueButtonUI.enabled = false;
        }
        else
        {
            //color button and enable interaction
            ContinueButtonUI.colors = originalColor;
        }
        
    }
}
