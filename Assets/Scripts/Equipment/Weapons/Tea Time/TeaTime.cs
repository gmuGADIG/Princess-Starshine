using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaTime : MonoBehaviour
{

    //If any of these variable names need changed feel free to change them to better match others
    //Or if needed feel free to remove some logic that doesn't fit, this applies to variables as well

    //The speed at which the projectile travels
    public float speed;
    //The time in seconds that the object stays alive before destroying itself
    public float aliveTime;
    //Tells the projectile whether it should die or not
    public bool useTimer;
    //The leniencyBox is the margin of error that a projectile can be away from the corner of the screen
    [Range(0f, 1f)]
    public float leniencyBox;
    //Radius of the circle collider that is enabled when the projectile hits a corner
    [Range(1f, 10f)]
    public float explosionRadius;
    //How long the object should stick around after it hits a corner
    public float deathTime;
    //The rigidbody of the projectile
    public Rigidbody2D rb;
    //The direction the projectile is going
    public Vector3 dir;
    //The explosion hit box
    public CircleCollider2D circle;
    //The damage the projectile does
    public float damage;
    
    //Internal timer shouldn't be touched
    private float timer;
    //Whether or not the object is currently moving
    private bool isMoving = true;
    //Stops the projectile from getting stuck in the corner
    private bool cooldown = false;
    void Start()
    {
        //Makes the explosion hitbox the specified size
        TryGetComponent<CircleCollider2D>(out circle);
        if(circle != null)
        {
            circle.radius = explosionRadius;
            circle.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //The logic for destroying the object after the specified time
        if (useTimer)
        {
            timer = timer + Time.deltaTime;
            if (timer > aliveTime)
            {
                Destroy(gameObject);
            }
        }
        //If moving then set the velocity to the current direction
        if (isMoving)
        {
            rb.velocity = dir * speed;
        }
        //Else it shouldn't move at all
        else
        {
            rb.velocity = Vector2.zero;
        }
        //Logic for whether the object has hit the corner of the screen
        Rect rect = cameraBoundingBox();
        if (!cooldown)
        {
            if (transform.position.x - ((transform.localScale.x / 2) + leniencyBox) <= rect.xMin ||
                transform.position.x + ((transform.localScale.x / 2) + leniencyBox) >= rect.xMax)
            {
                if (transform.position.y - ((transform.localScale.y / 2) + leniencyBox) <= rect.yMin ||
                    transform.position.y + ((transform.localScale.y / 2) + leniencyBox) >= rect.yMax)
                {
                    //Once it hits the corner it stops moving and enables the explosion hitbox then destroys itself after a given amount of time
                    isMoving = false;
                    if (circle)
                    {
                        circle.enabled = true;
                    }
                    StartCoroutine(countdown());
                    cooldown = true;
                }
            }
        }
        //If hit the left hand of the screen
        if(transform.position.x - (transform.localScale.x / 2) < rect.xMin)
        {
            dir.x = 1;
        }
        //If hit the right hand of the screen
        else if(transform.position.x + (transform.localScale.x / 2) > rect.xMax)
        {
            dir.x = -1;
        }
        //If hit the bottom of the screen
        else if (transform.position.y - (transform.localScale.y / 2) < rect.yMin)
        {
            dir.y = 1;
        }
        //If hit the top of the screen
        else if (transform.position.y + (transform.localScale.y / 2) > rect.yMax)
        {
            dir.y = -1;
        }
    }


    //Counts down in seconds, once done it will disable the explosion
    private IEnumerator countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(deathTime);
            circle.enabled = false;
            isMoving = true;
            cooldownC();
        }
    }

    private IEnumerator cooldownC()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            cooldown = false;
        }

    }

    //Use this method if you need the camera bounding box for anything else, This may be moved idk.
    //It returns the correct bounds for the camera based on world space.
    public static Rect cameraBoundingBox()
    {
        float camX = Camera.main.transform.position.x;
        float camY = Camera.main.transform.position.y;

        float verticalExt = Camera.main.orthographicSize;
        float horizontalExt = verticalExt * ((float)Screen.width / Screen.height);
        
        float minX = camX - horizontalExt;
        float maxX = camX + horizontalExt;
        float minY = camY - verticalExt;
        float maxY = camY + verticalExt;

        return new Rect(minX, minY, maxX*2, maxY*2);
    }
}
