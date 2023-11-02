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
    int cumulativeXpPoints = 0;
    int xpThisLevel = 0;
    int xpLevel = 1;
    int XpLevelUpGoal() => (2 * xpLevel) + 20;
    static Action<int, int> onLevelUp;

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
    float collisionRadius = .5f;

    RaycastHit2D[] collisions = new RaycastHit2D[50];

    // Time in seconds the player is immune to attacks. After getting hit, the player is immune for a short amount of time.
    // (in other words, i-frames)
    float immuneTime = 0f;

    //for animations
    public Animator playerAnimator;
    public Animator spriteRotator;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        curTwirlCharges = maxTwirlCharges;

        //Test the xp system with 10 xpPoints
        // AddXP(10);
        InGameUI.SetXp(0, 0);

        onLevelUp += (newLevel, xpThatLevel) => LevelUpUI.instance.Open();

        //sets player looking right
        ;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //check to see if the character is stationary or not

        if(input == Vector2.zero)
        {
            playerAnimator.SetBool("Moving", false);
        }
        else
        {
            playerAnimator.SetBool("Moving", true);

        }

        //check to see if the player is moving right or left

        if(Input.GetAxisRaw("Horizontal") >= 0)
        {
            spriteRotator.SetBool("MovingRight", true);

        }
        else
        {

            spriteRotator.SetBool("MovingRight", false);
        }

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

        immuneTime = Mathf.MoveTowards(immuneTime, 0, Time.deltaTime);
    }

    void UpdateTwirl(Vector2 input) {

        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("z")){
            if (curTwirlCharges > 0) {
                curTwirlCharges -= 1;
                StartCoroutine(Twirl(input));

                //do twirl animation

                playerAnimator.SetTrigger("Twirling");
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
        cumulativeXpPoints += points;
        xpThisLevel += points;

        var goal = XpLevelUpGoal();
        if (xpThisLevel >= goal)
        {
            xpThisLevel -= goal;
            xpLevel += 1;
            onLevelUp?.Invoke(cumulativeXpPoints, xpLevel);
        }

        InGameUI.SetXp(xpLevel,  (float) xpThisLevel / XpLevelUpGoal());
    }

    void OnCollision(RaycastHit2D hit) {
        //hit xp
        if (hit.collider.CompareTag("xp")) {
            var xpObj = hit.transform.gameObject.GetComponent<XpOrb>();
            AddXP(xpObj.points);
            Destroy(xpObj.gameObject);
        }
        
        else if (hit.collider.CompareTag("Enemy"))
        {
            if (immuneTime > 0 || isTwirling) return;
            OnAttacked(hit.collider.gameObject);
        }
    }

    void OnAttacked(GameObject enemy)
    {
        immuneTime = .5f;
        GetComponent<PlayerHealth>().decreaseHealth(10);
        print("oww my ass!");
    }
}
