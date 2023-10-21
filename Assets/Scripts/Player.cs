using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    
    // acceleration is by default set to 80, maxSpeed is set to 10, and deceleration is set to 30
    [HideInInspector] public Vector2 velocity = Vector2.zero;
    [SerializeField]
    float acceleration;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float deceleration;

    //for xp mechanic 
    //Serialization to be tested by user
    [SerializeField]
    int xpPoints = 0;
    //initially serialized for display purposes only
    int xpLevel = 0;
    Action<int> onLevelUp;

    //For dodge twirl
    public bool isTwirling = false;
    public int maxTwirlCharges = 3;
    public float twirlCooldown = 10f;
    public float twirlSpeed = 30;
    public float twirlDuration = 0.3f;

    private int curTwirlCharges = 0;
    private float twirlRechargeTimeLeft = 0f;
    //for collision function; radius is currently determined by player
    [SerializeField]
    float collisionRadius = 1;

    RaycastHit2D[] collisions = new RaycastHit2D[50];


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        curTwirlCharges = maxTwirlCharges;

        //Test the xp system with 10 xpPoints
        AddXP(10);
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (!isTwirling){
            if (input!=Vector2.zero) {
                velocity += input * acceleration * Time.deltaTime;
                velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
            }
            else
            {
                velocity = Vector2.MoveTowards(velocity,Vector2.zero,deceleration*Time.deltaTime);
            }
        }
        
        // twirl
        UpdateTwirl(input);

        // check collisions
        int hits = Physics2D.CircleCastNonAlloc(transform.position, collisionRadius, Vector2.zero, collisions);
        for (int i = 0; i < hits; i++)
        {
            OnCollision(collisions[i]);
        }

        transform.position += (Vector3)(velocity * Time.deltaTime);

    }

    void UpdateTwirl(Vector2 input) {

        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("z")){
            if (curTwirlCharges > 0) {
                curTwirlCharges -= 1;
                StartCoroutine(Twirl(input));
            }
            else {
                print("not enough charges!");
            }
        }

        twirlRechargeTimeLeft -= Time.deltaTime;
        if (twirlRechargeTimeLeft <= 0) {
            twirlRechargeTimeLeft = twirlCooldown;
            print("recharging twirl");
            curTwirlCharges = Mathf.Min(curTwirlCharges + 1, maxTwirlCharges);
        }
    }

    IEnumerator Twirl(Vector2 direction) {
        print($"twirling (twirls left = {curTwirlCharges})");
        isTwirling = true;
        velocity = direction * twirlSpeed;
        yield return new WaitForSeconds(twirlDuration);
        isTwirling = false;
    }

    //current placeholder for xp function
    void AddXP(int points) 
    {
        xpPoints += points;

        //placeholder for leveling up
        var startLevel = xpLevel;
        xpLevel = (int)Mathf.Sqrt(xpPoints);

        if (xpLevel > startLevel) {
            onLevelUp?.Invoke(xpLevel);
        }
    }

    void OnCollision(RaycastHit2D hit) {
        //hit xp
        if (hit.collider.CompareTag("xp")) {
            var xpObj = hit.transform.gameObject.GetComponent<XpOrb>();
            AddXP(xpObj.points);
            Destroy(xpObj.gameObject);
        }
    }
}
