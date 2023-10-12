using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public DifficultyManager difficulty;

    //Resarting the game on easy or normal as defined in the Diffuclty Manager will restart the game, and reset upgrades(Upgrade system not implemented yet)
    public void RestartButton() {

            //Restarts to the currentLevel
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    //if in hardmode the game will restart, it may be redundant since exit game effectivley does the same thing
    public void hardModeReset()
    {
            SceneManager.LoadScene("TitleScreenScene");


    }

    //Exits the game
    public void ExitButton()
    {

        SceneManager.LoadScene("TitleScreenScene");
       
    }


}


