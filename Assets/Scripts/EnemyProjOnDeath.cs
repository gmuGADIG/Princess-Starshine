using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjOnDeath : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    private GameObject player;
    public EnemyTemplate script;
    // Start is called before the first frame update
    void Start()
    {
        script = GetComponent<EnemyTemplate>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
    }
    void OnDestroy()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

}
