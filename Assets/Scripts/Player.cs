using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
    float cumulativeXpPoints = 0;
    float xpThisLevel = 0;
    int xpLevel = 1;
    
    [Tooltip("The initial amount of XP required for the player to level up.")]
    [SerializeField]
    int initialXpToLevelUp = 20;

    [Tooltip("The amount that the XP goal increases by per level.")]
    [SerializeField]
    int increaseXP = 2;

    [SerializeField]
    int XpLevelUpGoal() => (increaseXP * xpLevel) + initialXpToLevelUp;
    static Action<int, float> onLevelUp;

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

    List<RaycastHit2D> collisions = new(50);

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
    public string takeDamageSound { get; private set; } = "Princess_Damage";
    string levelUpSound = "Level_Up";
    string twirlDashSound = "Princess_Dash";

    public Action<Consumable?> PickedUpConsumable;
    
    Wall wall;

    public void Freeze() {
        SaveManager.SaveData.PlayerLevel = xpLevel;
        SaveManager.SaveData.PlayerXP = xpThisLevel;
        SaveManager.SaveData.HeldConsumable = heldConsumable;
    }

    public void Thaw() {
        xpLevel = SaveManager.SaveData.PlayerLevel;
        xpThisLevel = SaveManager.SaveData.PlayerXP;
        heldConsumable = SaveManager.SaveData.HeldConsumable;

        PickedUpConsumable?.Invoke(ConsumableManager.Instance.ConsumableOfConsumableType(heldConsumable));
    }

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
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

        // Update UIs
        InGameUI.SetXp(xpLevel, (float)xpThisLevel / XpLevelUpGoal());
        PickedUpConsumable?.Invoke(ConsumableManager.Instance.ConsumableOfConsumableType(heldConsumable));

        onLevelUp += (newLevel, xpThatLevel) => LevelUpUI.instance.Open();

        InGameUI.UpdateTwirls(curTwirlCharges);                

        wall = FindObjectOfType<Wall>();
    }

    void Update()
    {
        // Kill the player if a keybind is pressed
        if (Application.isEditor && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.L))
            GetComponent<PlayerHealth>().decreaseHealth(2 << 28); // is she hurt enough?

        // Almost kill the player if a keybind is pressed
        if (Application.isEditor && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.I)) {
            var health = GetComponent<PlayerHealth>();
            health.decreaseHealth(health.tempHealth - 1); // owchie
        }

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
        else if (facingDirection.x > 0)
            playerSprite.flipX = false;

        // Set velocity
        if (!isTwirling) {
            if (input != Vector2.zero) {
                velocity += input * acceleration * Time.deltaTime;
                velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

            } else {
                velocity = Vector2.MoveTowards(velocity,Vector2.zero,deceleration*Time.deltaTime);
            }
        }
        
        // twirl
        UpdateTwirl(input);

        //Check for camera bounds
        if (transform.position.y > camera.transform.position.y + constraintHeight && velocity.y>0)
            velocity.y = 0;
        else if (transform.position.y < camera.transform.position.y - constraintHeight && velocity.y < 0)
            velocity.y = 0;

        if (transform.position.x > camera.transform.position.x + constraintWidth && velocity.x > 0)
            velocity.x = 0;
        else if (transform.position.x < camera.transform.position.x - constraintWidth && velocity.x < 0)
            transform.position = new Vector2(camera.transform.position.x - constraintWidth, transform.position.y);

        // Handle wall bound
        if (wall != null) {
            if (transform.position.y > wall.Border && velocity.y > 0) {
                velocity.y = 0;
            }
        }

        UsedConsumable();

        // check collisions
        var filter = new ContactFilter2D();
        filter.NoFilter();

        int hits = Physics2D.CircleCast(
            transform.position - Vector3.back * 1000, 
            collisionRadius, 
            Vector2.zero, 
            filter,
            collisions
        );

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
        if (
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Z))
            && !isTwirling && input != Vector2.zero
        ){
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
    public void AddXP(float points, bool playSound = true) 
    {
        cumulativeXpPoints += points;
        xpThisLevel += points;
        if (playSound) {
            SoundManager.Instance.PlaySoundAtPosition(xpPickupSound, Camera.main.transform.position, Camera.main.transform);
        }

        var goal = XpLevelUpGoal();
        if (xpThisLevel >= goal)
        {
            xpThisLevel -= goal;
            xpLevel += 1;
            onLevelUp?.Invoke(xpLevel, cumulativeXpPoints);
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

        //else if (hit.collider.CompareTag("Enemy")|| hit.collider.CompareTag("WallOfFire") || hit.collider.CompareTag("EnemyProjectile"))
        else if (hit.collider.TryGetComponent<Damage>(out var damage) && damage.enabled)
        {
            if (isTwirling) return;
            if (immuneTime > 0) return;
            
            OnAttacked(damage.damage);
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
        //print("oww!");
    }
}
