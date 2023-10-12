using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    public DifficultyManager difficulty;

    //Resarting the game on easy or normal as defined in the Diffuclty Manager will restart the game, and reset upgrades(Upgrade system not implemented yet)
    public void RestartButton() {

        if(difficulty.currentDifficulty == DifficultyManager.DifficultyLevel.Easy || difficulty.currentDifficulty
            == DifficultyManager.DifficultyLevel.Normal)
        {
            //Restarts to the currentLevel
            SceneManager.LoadScene("UI");
            /*
             * upgrade = upgrades before the current level(Not yet implemented)
             * 
             */

        }

        //if in hardmode the game will restart, it may be redundant since exit game effectivley does the same thing
        if (difficulty.currentDifficulty == DifficultyManager.DifficultyLevel.Hard)
        {

            SceneManager.LoadScene("TitleScreenScene");


        }

    }

    
   
    //Exits the game
    public void ExitButton()
    {

        SceneManager.LoadScene("TitleScreenScene");
       
    }


}


