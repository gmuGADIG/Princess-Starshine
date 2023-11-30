using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Essentially the default enemy behavior, moves towards the player
 */
public class Thwomp : MonoBehaviour
{
    private GameObject player;
    Vector3 downVec;
    float originalY;
    bool hasHit;
    bool doRise;
    bool doFall;
    float wait;
    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        downVec = new Vector3(0, -10, 0);
        originalY = this.transform.position.y;
        doRise = false;
        doFall = false;
        hasHit = false;
        wait = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        if(player.transform.position.x >= this.transform.position.x - 1 && player.transform.position.x <= this.transform.position.x + 1 && player.transform.position.y < this.transform.position.y && doRise == false && doFall == false)
        { 
            Debug.Log("REEEEEEE");
            doFall = true;
        }
        if(doFall == true)
        {
            this.transform.position += downVec * Time.deltaTime;
        }
        if(player.transform.position.y >= this.transform.position.y)
        {
            hasHit = true;
            doFall = false;
        }
        if(hasHit == true)
        {
            wait -= 1 * Time.deltaTime;
            if(wait <= 0)
            {
                doRise = true;
                wait = 1;
            }

        }
        if(doRise == true)
        {
            this.transform.position -= downVec * Time.deltaTime;
            if(this.transform.position.y >= originalY)
            {
                doRise = false;
                hasHit = false;
            }
        }
    }

}
