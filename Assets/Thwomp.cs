using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Essentially the default enemy behavior, moves towards the player
 */
public class Thwomp : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void Update()
    {
        if(player.transform.position.x >= this.transform.position.x - 1 && player.transform.position.x <= this.transform.position.x + 1)
        {
            Debug.Log("REEEEEEE");
            Vector3.MoveTowards(this.transform.position, player.transform.position, 1);
        }
    }

}
