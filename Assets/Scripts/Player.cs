using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    
    // acceleration is by default set to 80, maxSpeed is set to 10, and deceleration is set to 30
    [HideInInspector] public Vector2 velocity = Vector2.zero;
    public BuffableStat moveSpeedMultiplier { get; private set; } = new BuffableStat(1f);
    //[HideInInspector] public float moveSpeedMultiplier { get; set; } = 1f;
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

    //Initial amount of xp required to level up
    [SerializeField]
    int initialXpToLevelUp = 20;

    //Increase amount in xp
    [SerializeField]
    int increaseXP = 2;

    [SerializeField]
    int XpLevelUpGoal() => (increaseXP * xpLevel) + initialXpToLevelUp;
    static Action<int, int> onLevelUp;

    //For dodge twirl
    public bool isTwirling = false;
    public int maxTwirlCharges = 3;
    public float twirlCooldown = 10f;
    public float twirlSpeed = 30;
    public float twirlDuration = 0.3f;

    private int curTwirlCharges = 0;
    private float twirlRechargeTimeLeft = 0f;

    private Consumable.Type heldConsumable = Consumable.Type.None;
    
    //for collision function; radius is currently determined by player
    [SerializeField]
    float collisionRadius = .5f;

    RaycastHit2D[] collisions = new RaycastHit2D[50];

    // Time in seconds the player is immune to attacks. After getting hit, the player is immune for a short amount of time.
    // (in other words, i-frames)

    //for animations
    public Animator playerAnimator;
    public Animator spriteRotator;

    //boolean to see where player is looking

    public enum Direction {Left,Right};
    Direction myDirection = Direction.Left;

    //find the player sprite renderer

    public SpriteRenderer mySpriteRenderer;

    // sound names
    string xpPickupSound = "XP_Pickup";
    string takeDamageSound = "Princess_Damage";
    string levelUpSound = "Level_Up";
    string twirlDashSound = "Princess_Dash";


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //heldConsumable = Consumable.Type.LevelUp; // damn foot gun

        curTwirlCharges = maxTwirlCharges;

        //Test the xp system with 10 xpPoints
        // AddXP(10);
        InGameUI.SetXp(0, 0);

        onLevelUp += (newLevel, xpThatLevel) => LevelUpUI.instance.Open();

        //sets player looking right
        Direction myDirection = Direction.Left;

        
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
        //chnages where the player is looking
        if (Input.GetAxisRaw("Horizontal") < 0)
        {

            myDirection = Direction.Left;
             mySpriteRenderer.flipX = true;

        }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            myDirection = Direction.Right;
            mySpriteRenderer.flipX = false;
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

        UsedConsumable();

        // check collisions
        int hits = Physics2D.CircleCastNonAlloc(transform.position, collisionRadius, Vector2.zero, collisions);
        for (int i = 0; i < hits; i++)
        {
            OnCollision(collisions[i]);
        }

        transform.position += (Vector3)(velocity * Time.deltaTime * moveSpeedMultiplier.Value);
    }

    void UsedConsumable()
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown("x"))
        {
            if (heldConsumable != Consumable.Type.None && Consumable.CanApply(heldConsumable))
            {
                Consumable.Apply(heldConsumable);
                Debug.Log("Player.cs: Consumed Successfully");
                heldConsumable = Consumable.Type.None;
            }
        }
    }

    void UpdateTwirl(Vector2 input) {

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Z)){
            if (curTwirlCharges > 0) {
                curTwirlCharges -= 1;
                InGameUI.UpdateTwirls(curTwirlCharges);
                StartCoroutine(Twirl(input));

                //do twirl animation
                playerAnimator.SetTrigger("Twirling");
                SoundManager.Instance.PlaySoundGlobal(twirlDashSound);
            }
            else {
                print("not enough charges!");
            }
        }

        twirlRechargeTimeLeft -= Time.deltaTime;
        if (twirlRechargeTimeLeft <= 0) {
            twirlRechargeTimeLeft = twirlCooldown;
            print("recharging twirl");
            if (curTwirlCharges < maxTwirlCharges)
            {
                curTwirlCharges += 1;
                InGameUI.UpdateTwirls(curTwirlCharges);                
            }
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
    public void AddXP(int points) 
    {
        cumulativeXpPoints += points;
        xpThisLevel += points;
        SoundManager.Instance.PlaySoundGlobal(xpPickupSound);

        var goal = XpLevelUpGoal();
        if (xpThisLevel >= goal)
        {
            xpThisLevel -= goal;
            xpLevel += 1;
            onLevelUp?.Invoke(cumulativeXpPoints, xpLevel);
            SoundManager.Instance.PlaySoundGlobal(levelUpSound);
        }

        InGameUI.SetXp(xpLevel,  (float) xpThisLevel / XpLevelUpGoal());
    }

    // immediately levels up the player by giving them the required XP (idk what im doing :P)
    public void LevelUp() {
        AddXP(XpLevelUpGoal() - xpThisLevel);
    }

    void OnCollision(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("xp")) {
            var xpObj = hit.transform.gameObject.GetComponent<XpOrb>();
            AddXP(xpObj.points);
            Destroy(xpObj.gameObject);
        }

        else if (hit.collider.CompareTag("Enemy"))
        {
            if (isTwirling) return;
            OnAttacked(hit.collider.gameObject.GetComponent<Damage>().damage);
        }

        else if (hit.collider.CompareTag("Consumable"))
        {
            Consumable consumable = hit.collider.gameObject.GetComponent<Consumable>();

            // if we already have a consumable, stop here
            if (heldConsumable != Consumable.Type.None && consumable.ConsumableType != Consumable.Type.Health)
            {
                return;
            }

            Debug.Log("Hit consumable " + consumable.ConsumableType);

            // if the consumable is health, use it now
            if (consumable.ConsumableType == Consumable.Type.Health)
            {
                Consumable.Apply(Consumable.Type.Health);
            }
            else // otherwise just hold onto it
            {
                heldConsumable = consumable.ConsumableType;
            }

            // destroy any consumables we "consume"
            GameObject.Destroy(hit.collider.gameObject);
        }
    }

    //void OnAttacked(GameObject enemy)
    void OnAttacked(float damage)
    {
        GetComponent<PlayerHealth>().decreaseHealth(damage * Time.deltaTime);
        SoundManager.Instance.PlaySoundGlobal(takeDamageSound);
        //print("oww!");
    }
}
