using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeaTime : Projectile
{

    //If any of these variable names need changed feel free to change them to better match others
    //Or if needed feel free to remove some logic that doesn't fit, this applies to variables as well

    //The speed at which the projectile travels
    public float speed;
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
    //The direction the projectile is going
    public Vector2 dir;
    //The direction of the projectile but locked at positive values.
    private Vector2 absDirection;
    //The explosion hitbox
    private CircleCollider2D circle;
    
    //Internal timer shouldn't be touched
    private float timer;
    //Whether or not the object is currently moving
    private bool isMoving = true;
    //Stops the projectile from getting stuck in the corner
    private bool cooldown = false;
    protected override void Start()
    {
        base.Start();
        //Makes the explosion hitbox the specified size
        TryGetComponent<CircleCollider2D>(out circle);
        if(circle != null)
        {
            circle.radius = explosionRadius;
            circle.enabled = false;
        }
    }

    //The setup for the projectile to make it usable
    public override void Setup(ProjectileWeapon weapon, Vector2 target, float damage, int pierceCount, float speed, float knockback, float size)
    {
        this.weapon = weapon;
        dir = (target - (Vector2)this.transform.position).normalized;
        absDirection = dir.Abs();
        this.damage = damage; 
        this.speed = speed;
        this.knockback = knockback;
        transform.localScale = new Vector2(size, size);
        this.pierceCount = pierceCount;
        hasBeenSetUp = true;
    }

    //Change stats once the weapon is leveled up
    public override void OnWeaponLevelUp(float newDamage, int newPierceCount, float newSpeed, float newKnockback, float newSize) 
    {
        damage = newDamage;
        pierceCount = newPierceCount;
        speed = newSpeed;
        knockback = newKnockback;
        transform.localScale = new Vector2(newSize, newSize);    
    }

    // Update is called once per frame
    new void Update()
    {
        //The logic for destroying the object after the specified time
        if (useTimer)
        {
            timer = timer + Time.deltaTime;
            if (timer > maxLifeTime)
            {
                Destroy(gameObject);
            }
        }
        //If moving then set the velocity to the current direction
        if (isMoving)
        {
            transform.position += (Vector3)(dir * speed * Time.deltaTime);
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
            dir.x = absDirection.x;
        }
        //If hit the right hand of the screen
        else if(transform.position.x + (transform.localScale.x / 2) > rect.xMax)
        {
            dir.x = (-absDirection.x);
        }
        //If hit the bottom of the screen
        else if (transform.position.y - (transform.localScale.y / 2) < rect.yMin)
        {
            dir.y = absDirection.y;
        }
        //If hit the top of the screen
        else if (transform.position.y + (transform.localScale.y / 2) > rect.yMax)
        {
            dir.y = (-absDirection.y);
        }
    }


    //Counts down in seconds, once done it will disable the explosion
    private IEnumerator countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(deathTime);
            //remove this if it is supposed to persist after hitting a corner
            Destroy(gameObject);


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
        
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * ((float)Screen.width / Screen.height);
        
        return new Rect(camX - halfWidth, camY - halfHeight, halfWidth * 2, halfHeight * 2);
        
        // var cam = Camera.main;
        //
        // var camPos = cam.transform.position;
        // camPos.z = 0;
        //
        // float halfHeight = cam.orthographicSize;
        // float halfWidth = halfHeight * Screen.width / Screen.height;
        //
        // var bottomLeft = camPos - new Vector3(halfWidth, halfHeight);
        // var topRight = camPos + new Vector3(halfWidth, halfHeight);
        // return new Rect(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
    }
}
