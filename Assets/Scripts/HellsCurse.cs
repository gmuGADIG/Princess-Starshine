using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HellsCurse : MonoBehaviour
{

    /*
     * DoD: Between levels, user must choose one upgrade or weapon to discard. 
     * The chosen weapon or upgrade must be at a level equal to or greater than the level the player just finished. 
     * (If the player just beat level 2, they must select a level 2 or higher weapon/upgrade to discard, and so on.) 
     * In hard mode, Hell's Curse automatically chooses which weapons/upgrades to discard.
     */
    public bool inHardMode = false;
    public bool debugBool;
    List<item> SelectableItems = new List<item>();
    private item SelectedItem;
    private void Start()
    {
        //get difficulty
        if(debugBool)
        {
            inHardMode = true;
        }
        else
        {
            inHardMode = false;
        }
        //get level (i.e. 1, 2, 3..)
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // first scene (0) is title screen, second scene (1) would be first level etc
        //get player upgrades and weapons
    }

    private void ChooseItem()
    {
        //search inventory for valid items
        foreach (item in inventory)
        {
            if(item.level >= currentLevel)
            {
                SelectableItems.Add(item);
            }
        }
        //prompt the player to choose a valid item
        int intInput = 0;
        if(Input.GetKey("1"))
        {
            intInput = 1;
        }
        else if (Input.GetKey("2"))
        {
            intInput = 2;
        }
        else if (Input.GetKey("3"))
        {
            intInput = 3;
        }
        else if (Input.GetKey("4"))
        {
            intInput = 4;
        }
        else if (Input.GetKey("5"))
        {
            intInput = 5;
        }
        else if (Input.GetKey("6"))
        {
            intInput = 6;
        }
        else if (Input.GetKey("7"))
        {
            intInput = 7;
        }

        printf(intInput);

        //remove selected item
        inventory.Remove(SelectedItem);

    }

        




}
