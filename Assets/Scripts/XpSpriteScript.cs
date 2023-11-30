using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpSpriteScript : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    // Start is called before the first frame update
    void Start()
    {

        //choose a random sprite to be

        int spriteNumber = Random.Range(1, 3);

        if(spriteNumber == 2)
        {

            spriteRenderer.sprite = sprite2;

        }

        if (spriteNumber == 3)
        {

            spriteRenderer.sprite = sprite3;

        }

        //make each a random size

        float value = Random.Range(0.075f, 0.15f);

        gameObject.transform.localScale = new Vector3(value,value,value);


        //make each a random rotation


        transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
