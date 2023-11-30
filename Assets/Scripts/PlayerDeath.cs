using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerDeath : MonoBehaviour
{
    public PlayerHealth health;
    //public DifficultyManager difficulty;
    public GameOverScript gameOver;
    /* Unsure of how lives and difficulty is supposed to work */
    //public int lives = 3;
    private void Start()
    {
        LoadObjects();
    }
    private void Update()
    {
        if (health.isDead)
        {
            if (gameOver != null)
            {
                gameOver.DoGameOver();
            }
        }
    }


    // Reset is called when the script is loaded or reset in the inspector
    private void Reset()
    {
        LoadObjects();
    }

    private void LoadObjects()
    {
        if(health == null)
        {
            if(TryGetComponent(out PlayerHealth health))
            {
                this.health = health;
            } else
            {
                Debug.LogError("PlayerDeath requires a PlayerHealth component.");
            }
        }
        if (gameOver == null)
        {
            GameOverScript gameOver = FindObjectOfType<GameOverScript>(true);
            if(gameOver == null)
            {
                Debug.LogError("PlayerDeath requires a GameOverScript in the scene.");
            } else
            {
                this.gameOver = gameOver;
            }
        }
    }
}
