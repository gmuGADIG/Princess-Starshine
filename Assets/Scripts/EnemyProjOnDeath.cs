using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjOnDeath : MonoBehaviour
{
    public GameObject bullet;
    private Transform enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        enemyPos = gameObject.GetComponent<Transform>();
    }

    void OnDisable()
    {
        if(!this.gameObject.scene.isLoaded) return; // Prevents instatiating bullet when scene isn't loaded
        Instantiate(bullet, enemyPos.position, Quaternion.identity);
    }

}
