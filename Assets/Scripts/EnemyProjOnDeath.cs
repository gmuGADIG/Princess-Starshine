using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjOnDeath : MonoBehaviour
{
    public GameObject bullet;

    void OnDisable()
    {
        if(!this.gameObject.scene.isLoaded) return; // Prevents instatiating bullet when scene isn't loaded
        Instantiate(bullet, transform.position, Quaternion.identity);
    }

}
