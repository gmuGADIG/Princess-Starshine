using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
   
        public static EnemyScript Instance;

        private float spawnRate = 2f; // Default enemy spawn rate

        private void Awake()
        {
            Instance = this;
        }

        public void SetSpawnRate(float newRate)
        {
            spawnRate = newRate;
        }
    
}