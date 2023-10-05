using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   

    // accleeration is by default set to 80, maxSpeed is set to 10, and deceleration is set to 30
    Vector2 velocity = Vector2.zero;
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

    //For dodge twirl
    
    public int maxTwirlCharges = 3;
    public float twirlCooldown = 10f;
    public float twirlSpeed = 100;
    public float twirlDuration = 0.7f;

    private int curTwirlCharges = 0;
    private float twirlRechargeTimeLeft = 0f;
    //for collision function; radius is currently determined by player
    [SerializeField]
    float collisionRadius = 1;

    // Start is called before the first frame update
    void Start() 
    {   
        curTwirlCharges = maxTwirlCharges;

        //Test the xp system with 10 xpPoints
        AddXP(10);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (input!=Vector2.zero) {
            velocity += input * acceleration * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        }
        else
        {
            velocity = Vector2.MoveTowards(velocity,Vector2.zero,deceleration*Time.deltaTime);
        }
        transform.position += (Vector3)(velocity * Time.deltaTime);

        // twirl
        UpdateTwirl(input);

        // check collisions
        var collisions = Physics2D.CircleCastAll(transform.position, collisionRadius, Vector2.zero);
        foreach (var hit in collisions) {
            OnCollision(hit);
        }
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
        velocity = direction * twirlSpeed;
        yield return new WaitForSeconds(twirlDuration);
        velocity = Vector2.zero;
    }

    //current placeholder for xp function
    void AddXP(int points) 
    {
        xpPoints += points;

        //placeholder for leveling up
        xpLevel = (int)Mathf.Sqrt(xpPoints);
        
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
