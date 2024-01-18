using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {
    //public DifficultyManager difficulty;
    public bool IsGameOver { get; private set;}
    public GameObject gameOverScreen;

    private void Start() {
        gameOverScreen.SetActive(false);
    }

    public void DoGameOver() {
        LoadObjects();
        IsGameOver = true;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0; // pause the game while gameovered
        FindObjectOfType<BossHealthBarUI>()?.gameObject.SetActive(false); // Disable boss screen
    }
    //Resarting the game on easy or normal as defined in the Diffuclty Manager will restart the game, and reset upgrades(Upgrade system not implemented yet)
    public void RestartButton() {

            //Restarts to the currentLevel
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
           
    }

    //if in hardmode the game will restart, it may be redundant since exit game effectivley does the same thing
    public void hardModeReset() {
            SceneManager.LoadScene("TitleScreenScene");


    }

    //Exits the game
    public void ExitButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreenScene");
       
    }

    private void Reset() {
        LoadObjects();
    }
    private void LoadObjects() {
        if (gameOverScreen == null) {
            gameOverScreen = this.gameObject;//theoretically should be the same
        }
    }

}


