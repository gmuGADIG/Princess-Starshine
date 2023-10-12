using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

    public DifficultyLevel currentDifficulty = DifficultyLevel.Normal;
    public float enemySpawnRate = 2f; // Default enemy spawn rate
    public float projectileFireRate = 0.5f; // Default projectile fire rate

    private void Start()
    {
        SetDifficulty(currentDifficulty);
    }

    public void SetDifficulty(DifficultyLevel newDifficulty)
    {
        currentDifficulty = newDifficulty;

        // Adjust gameplay parameters based on difficulty
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                enemySpawnRate = 1f; // Slower enemy spawn rate
                projectileFireRate = 0.7f; // Slower projectile fire rate
                break;
            case DifficultyLevel.Normal:
                enemySpawnRate = 2f; // Standard enemy spawn rate
                projectileFireRate = 0.5f; // Standard projectile fire rate
                break;
            case DifficultyLevel.Hard:
                enemySpawnRate = 3f; // Faster enemy spawn rate
                projectileFireRate = 0.3f; // Faster projectile fire rate
                break;
        }

        // Apply the adjusted parameters to the game objects
        EnemyScript.Instance.SetSpawnRate(enemySpawnRate);
        
    }
}
