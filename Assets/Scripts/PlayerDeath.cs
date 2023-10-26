using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public PlayerHealth health;
    public DifficultyManager difficulty;
    public GameOverScript gameOver;
    public static int life = 3;


    public void Death(DifficultyManager difficulty)
    {
        if (health.isDead)

        {
            if (difficulty.currentDifficulty == DifficultyManager.DifficultyLevel.Hard)
            {

                gameOver.hardModeReset();


            }

            else {

                life--;
                gameOver.RestartButton();


            }

        }
    }
}
