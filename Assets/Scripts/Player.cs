using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player instance;
    
    // acceleration is by default set to 80, maxSpeed is set to 10, and deceleration is set to 30
    [HideInInspector] public Vector2 velocity = Vector2.zero;
    public BuffableStat moveSpeedMultiplier { get; private set; } = new BuffableStat(1f);
    //[HideInInspector] public float moveSpeedMultiplier { get; set; } = 1f;
    [Tooltip("The player's acceleration.")]
    [SerializeField]
    float acceleration;
    [Tooltip("The maximum speed the player will reach.")]
    [SerializeField]
    float maxSpeed;
    [Tooltip("The rate at which the player will decelerate.")]
    [SerializeField]
    float deceleration;

    //For preventing player from leaving the camera
    private float constraintHeight;
    private float constraintWidth;
    //private float worldHeight;
    //private float worldWidth;
    private Camera camera;
    //private float playerHeight;
    //private float playerWidth;

    //for xp mechanic 
    int cumulativeXpPoints = 0;
    int xpThisLevel = 0;
    int xpLevel = 1;
    
    [Tooltip("The initial amount of XP required for the player to level up.")]
    [SerializeField]
    int initialXpToLevelUp = 20;

    [Tooltip("The amount that the XP goal increases by per level.")]
    [SerializeField]
    int increaseXP = 2;

    [SerializeField]
    int XpLevelUpGoal() => (increaseXP * xpLevel) + initialXpToLevelUp;
    static Action<int, int> onLevelUp;

    //For dodge twirl
    public bool isTwirling = false;
    [Tooltip("The maximum number of twirl charges the player can have saved up.")]
    public int maxTwirlCharges = 3;
    [Tooltip("The cooldown of each twirl, in seconds.")]
    public float twirlCooldown = 10f;
    [Tooltip("How fast the player moves while twirling.")]
    public float twirlSpeed = 30;
    [Tooltip("How long the twirl lasts in seconds.")]
    public float twirlDuration = 0.3f;

    private int curTwirlCharges = 0;
    private float twirlRechargeTimeLeft = 0f;

    public Action PlayerTwirled;

    private Consumable.Type heldConsumable = Consumable.Type.None;
    
    //for collision function; radius is currently determined by player
    [SerializeField]
    float collisionRadius = .5f;

    RaycastHit2D[] collisions = new RaycastHit2D[50];

    // Time in seconds the player is immune to attacks. After getting hit, the player is immune for a short amount of time.
    // (in other words, i-frames)
    float immuneTime = 0f;
    const float ImmunityTimeBetweenHits = 0.5f;

    //for animations
    public Animator playerAnimator;
    
    // normalized direction the player is walking, or if they're still, the last walking direction.
    public Vector2 facingDirection = Vector2.right;

    //find the player sprite renderer
    public SpriteRenderer playerSprite;

    // sound names
    string xpPickupSound = "XP_Pickup";
    string takeDamageSound = "Princess_Damage";
    string levelUpSound = "Level_Up";
    string twirlDashSound = "Princess_Dash";

    public Action<Consumable?> PickedUpConsumable;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        camera = Camera.main;
        float aspectRatio = (float)Screen.width / Screen.height;
        float worldHeight = camera.orthographicSize * 2;
        float worldWidth = worldHeight * aspectRatio;
        float playerHeight = gameObject.transform.localScale.y;
        float playerWidth = gameObject.transform.localScale.x;
        constraintHeight = worldHeight / 2 - playerHeight / 2;
        constraintWidth = worldWidth / 2 - playerWidth / 2;

        //heldConsumable = Consumable.Type.LevelUp; // damn foot gun

        curTwirlCharges = maxTwirlCharges;

        //Test the xp system with 10 xpPoints
        // AddXP(10);
        InGameUI.SetXp(0, 0);

        onLevelUp += (newLevel, xpThatLevel) => LevelUpUI.instance.Open();

        InGameUI.UpdateTwirls(curTwirlCharges);                
    }

    void Update()
    {
        // Kill the player if a keybind is pressed
        if (Application.isEditor && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.L))
            GetComponent<PlayerHealth>().decreaseHealth(float.PositiveInfinity); // is she hurt enough?

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
        //changes where the player is looking
        if (velocity != Vector2.zero) facingDirection = velocity.normalized;
        if (facingDirection.x < 0)
            playerSprite.flipX = true;
        else if(facingDirection.x > 0)
            playerSprite.flipX = false;

        if (!isTwirling){
            if (input!=Vector2.zero) {
                velocity += input * acceleration * Time.deltaTime;
                velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

                //Check for camera bounds
                if (gameObject.transform.position.y>camera.transform.position.y+constraintHeight&& velocity.y>0)
                    velocity.y = 0;
                else if (gameObject.transform.position.y < camera.transform.position.y - constraintHeight && velocity.y < 0)
                    velocity.y = 0;

                if (gameObject.transform.position.x > camera.transform.position.x + constraintWidth && velocity.x > 0)
                    velocity.x = 0;
                else if (gameObject.transform.position.x < camera.transform.position.x - constraintWidth && velocity.x < 0)
                    gameObject.transform.position = new Vector2(camera.transform.position.x - constraintWidth, gameObject.transform.position.y);
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

        immuneTime -= Time.deltaTime;
        if (immuneTime < 0) immuneTime = 0;
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
                PickedUpConsumable?.Invoke(null);
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

                PlayerTwirled?.Invoke();
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

        else if (hit.collider.CompareTag("Enemy")|| hit.collider.CompareTag("WallOfFire") || hit.collider.CompareTag("EnemyProjectile"))
        {
            if (isTwirling) return;
            if (immuneTime > 0) return;
            
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
                PickedUpConsumable?.Invoke(consumable);
            }

            // destroy any consumables we "consume"
            GameObject.Destroy(hit.collider.gameObject);
        }
    }

    //void OnAttacked(GameObject enemy)
    void OnAttacked(float damage)
    {
        immuneTime = ImmunityTimeBetweenHits;
        GetComponent<PlayerHealth>().decreaseHealth(damage);
        SoundManager.Instance.PlaySoundGlobal(takeDamageSound);
        //print("oww!");
    }
}
