using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager enemyManager;

    public int maxEnemies = 10;
    public HashSet<GameObject> enemies;

    void Awake() {
        if (enemyManager == null) {
            enemyManager = this;
        }
        else {
            return;
        }

        enemies = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));//TODO: Enemies need to add and remove themselves from this list
    } 
}