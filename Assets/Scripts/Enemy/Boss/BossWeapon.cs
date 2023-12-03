using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

enum BossWeaponType
{
    MachineGun,
    Puddle,
    Orbit,
    Melee,
    EnemySpawn,
    None
}

public enum BossMovementType
{
    Wander,
    Flee,
    Agressive,
    None
}


public enum BossEnemySpawnLocation
{
    OffScreen,
    OnScreen,
    OnBoss,
    InCorner,
    None
}

[Serializable]
public struct MachineGun
{
    [Tooltip("The damage the machine gun does")]
    public float machineGunDamage;
    [Tooltip("The time in seconds to fire a projectile")]
    public float fireRate;
    [Tooltip("The Amount of bullets that the attack will have")]
    public int bulletQuantity;
    [Tooltip("The speed of the projectile")]
    public float bulletSpeed;
    [Tooltip("The spread of the projectiles in degrees, although the actual spread is the angle*2 so 20 gives 40 degrees of spread")]
    public float bulletSpread;
    [Tooltip("If this is enabled the machine gun will target the player otherwise it will shoot towards the middle of the screen")]
    public bool aimAtPlayer;
    [Tooltip("The offset from the boss to spawn the projectile")]
    public bool shootFromCorner;
    [Tooltip("The GameObject the machine gun fires")]
    public GameObject machineGunProjectile;
}

[Serializable]
public struct Puddle
{
    [Tooltip("How fast the puddles are spawned in seconds")]
    public float puddleSpawnTime;
    [Tooltip("How long a puddles exists before destroying itself")]
    public float puddleAliveTime;
    [Tooltip("The damage a puddle does")]
    public float puddleDamage;
    [Tooltip("Enable if the puddles should spawn on the player")]
    public bool spawnOnPlayer;
    [Tooltip("Enable if the puddles should spawn on the boss")]
    public bool spawnOnBoss;
    [Tooltip("The amount of puddles to spawn")]
    public int spawnQuantity;
    [Tooltip("How much the size of the puddle changes over time")]
    public float puddleSizeChange;
    [Tooltip("Enable if the puddle should change in size after the damage delay")]
    public bool puddleSizeChangeAfterDelay;
    [Tooltip("The time until the hitbox is enabled")]
    public float damageDelay;
    [Tooltip("The puddle object")]
    public GameObject puddleObject;
}

[Serializable]
public struct Orbit
{
    [Tooltip("The damage the projectile")]
    public float orbitDamage;
    [Tooltip("The amount of time the projectile takes to go around the circle: SMALL TIMES LOOK BROKEN")]
    public float projectileTime;
    [Tooltip("The amount of projectiles that should orbit the boss")]
    public int projectileQuantity;
    [Tooltip("The distance from the boss")]
    public float distanceFromBoss;
    [Tooltip("The distance change from the boss over time")]
    public float distanceChangeTime;
    [Tooltip("The change in time for a revolution of the orbit, this is the final time that the projectiles will go to")]
    public float finalRevolution;
    [Tooltip("The amount the period should change over time to get to final revolution")]
    public float revolutionChangeTime;
    [Tooltip("Enable if the changes over time should happen")]
    public bool doStatChanges;
    [Tooltip("The time till the stat changes start occuring")]
    public float timeTilChanges;
    [Tooltip("The amount of time the projectiles will stay around")]
    public float duration;
    [Tooltip("The orbit projectile")]
    public GameObject orbitObject;
}

[Serializable]
public struct Melee
{
    public float meleeDamage;
    public float radius;
    public float hitboxTime;
    public float speed;
    public int numberRepeat;
    //Will be changed to enum
    public BossMovementType movementType;
}

[Serializable]
public struct EnemySpawn
{
    public BossEnemySpawnLocation enemySpawnLocation;
    public GameObject enemy;
    public int enemyQuantity;
    public float additionalDelay;
}

public class BossWeapon : MonoBehaviour
{

    public float timeBetweenAttacks;
    [Header("Machine Gun")]
    public bool machineGunEnabled;
    public float machineGunFrequency;
    public MachineGun machinegun;
    private float shotTimer;
    private int bulletCount;

    [Header("Puddle")]
    public bool puddleEnabled;
    public float puddleFrequency;
    public Puddle puddle;
    private float puddleSpawnTimer;
    private int puddleCount;

    [Header("Orbit")]
    public bool orbitEnabled;
    public float orbitFrequency;
    public Orbit orbit;
    private bool orbitSpawned;
    private float orbitDurationTimer;

    [Header("Melee")]
    public bool meleeEnabled;
    public float meleeFrequency;
    public Melee melee;
    public CircleCollider2D meleeCollider;
    private float speedTimer;
    private float hitboxTimer;
    private int repeatCount;
    private IBossMovement meleeMovement;


    [Header("Enemy Spawn")]
    public bool enemySpawnEnabled;
    public float enemySpawnFrequency;
    public EnemySpawn enemySpawn;
    private float additionalDelayTimer;
    private bool spawnedEnemies;

    private float frequencyTotal;
    private float attackTimer = 0;
    private BossWeaponType currentWeapon = BossWeaponType.None;
    private bool isAttacking = false;
    private GameObject target;
    private IBossMovement startMovement;
   


    void Start()
    {
        if(puddle.spawnOnBoss && puddle.spawnOnPlayer)
        {
            Debug.LogError("PUDDLE WEAPON: Spawning isn't set correctly only select one of the spawn on boxes");
        }
        if(melee.numberRepeat == 0)
        {
            melee.numberRepeat = 1;
        }
        if(!machineGunEnabled)
        {
            machineGunFrequency = 0;
        }
        if(!puddleEnabled)
        {
            puddleFrequency = 0;
        }
        if(!orbitEnabled)
        {
            orbitFrequency = 0;
        }
        if (!meleeEnabled)
        {
            meleeFrequency = 0;
        }
        if(!enemySpawnEnabled)
        {
            enemySpawnFrequency = 0;
        }
        frequencyTotal = machineGunFrequency + puddleFrequency + orbitFrequency + meleeFrequency + enemySpawnFrequency;
        puddleFrequency += machineGunFrequency;
        orbitFrequency += puddleFrequency;
        meleeFrequency += orbitFrequency;
        enemySpawnFrequency += meleeFrequency;
        if(frequencyTotal == 0)
        {
            Debug.LogError("Nothing is Enabled or has a frequency");
        }
        if (meleeEnabled)
        {
            meleeCollider.enabled = false;
            meleeCollider.radius = melee.radius;
        }
        if(!TryGetComponent<IBossMovement>(out startMovement))
        {
            Debug.LogError("BOSS HAS NO MOVEMENT");
        }
        switch (melee.movementType)
        {
            case BossMovementType.Agressive:
                meleeMovement = gameObject.AddComponent<BossMovementAggressive>();
                break;
            case BossMovementType.Wander:
                meleeMovement = gameObject.AddComponent<BossMovementWander>();
                break;
            case BossMovementType.Flee:
                meleeMovement = gameObject.AddComponent<BossMovementFlee>();
                break;
            case BossMovementType.None:
                meleeMovement = null;
                break;
            default:
                break;
        }
        if (meleeMovement == null) { }
        else
        {
            meleeMovement.enabled = false;
        }
    }

    void Update()
    {
        if(attackTimer < timeBetweenAttacks)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        if (!isAttacking)
        {
            float randomNumber = UnityEngine.Random.Range(0.0f, frequencyTotal);
            if (randomNumber <= machineGunFrequency && machineGunEnabled)
            {
                currentWeapon = BossWeaponType.MachineGun;
                bulletCount = 0;
                if (machinegun.aimAtPlayer)
                {
                    GetPlayerTarget();
                }
                else
                {
                    Rect camBox = TeaTime.cameraBoundingBox();
                    target = new GameObject();
                    target.transform.position = camBox.center;
                }
                //Machine Gun
            }
            else if (randomNumber <= puddleFrequency && puddleEnabled)
            {
                currentWeapon = BossWeaponType.Puddle;
                puddleCount = 0;
                if (puddle.spawnOnPlayer)
                {
                    GetPlayerTarget();
                }
                //Puddle
            }
            else if (randomNumber <= orbitFrequency && orbitEnabled)
            {
                currentWeapon = BossWeaponType.Orbit;
                orbitDurationTimer = 0;
                orbitSpawned = false;
                //Orbit
            }
            else if (randomNumber <= meleeFrequency && meleeEnabled)
            {
                currentWeapon = BossWeaponType.Melee;
                repeatCount = 0;
                startMovement.enabled = false;
                if (meleeMovement == null) { }
                else
                {
                    meleeMovement.enabled = true;
                }
                //melee
            }
            else if (randomNumber <= enemySpawnFrequency && enemySpawnEnabled)
            {
                currentWeapon = BossWeaponType.EnemySpawn;
                additionalDelayTimer = 0;
                spawnedEnemies = false;
                //Enemy Spawns
            }
            else
            {
                Debug.LogError("Somehow the random generator broke");
            }
            isAttacking = true;
        }
        else
        {
            switch (currentWeapon)
            {
                case BossWeaponType.MachineGun:
                    MachineGun();
                    break;
                case BossWeaponType.Puddle:
                    if (puddle.spawnOnPlayer)
                        PuddleOnPlayer();
                    else if (puddle.spawnOnBoss) 
                        PuddleOnBoss();
                    else
                        RandomPuddle();
                    break;
                case BossWeaponType.Orbit:
                    Orbit();
                    break;
                case BossWeaponType.Melee:
                    Melee();
                    break;
                case BossWeaponType.EnemySpawn:
                    EnemySpawn();
                    break;
                case BossWeaponType.None:
                    break;
            }
        }
    }

    private void MachineGun()
    {
        if(shotTimer < machinegun.fireRate)
        {
            shotTimer += Time.deltaTime;
            return;
        }
        else
        {
            if(machinegun.bulletQuantity < bulletCount)
            {
                isAttacking = false;
                attackTimer = 0;
                return;
            }
            else
            {
                if (!machinegun.shootFromCorner)
                {
                    Vector3 currentPos = transform.position;
                    Vector3 targetPos = target.transform.position;
                    Vector2 triangle = currentPos - targetPos;
                    float angle = Mathf.Atan2(-triangle.y, -triangle.x);
                    float fireAngle = UnityEngine.Random.Range(angle - (Mathf.Deg2Rad * machinegun.bulletSpread), angle + (Mathf.Deg2Rad * machinegun.bulletSpread));
                    Vector2 velocity = new Vector2(Mathf.Cos(fireAngle), Mathf.Sin(fireAngle));

                    GameObject proj = Instantiate(machinegun.machineGunProjectile);
                    proj.transform.position = currentPos;
                    BossProjectile bossProjectile;
                    proj.TryGetComponent<BossProjectile>(out bossProjectile);
                    if (bossProjectile)
                    {
                        bossProjectile.Setup(velocity, machinegun.machineGunDamage, machinegun.bulletSpeed, 5);
                    }
                    bulletCount++;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Rect camBox = TeaTime.cameraBoundingBox();
                        Vector3 currentPos;
                        switch (i) {
                            case 0:
                                currentPos = new Vector3(camBox.xMin, camBox.yMin);
                                break;
                            case 1:
                                currentPos = new Vector3(camBox.xMin, camBox.yMax);
                                break;
                            case 2:
                                currentPos = new Vector3(camBox.xMax, camBox.yMax);
                                break;
                            case 3:
                                currentPos = new Vector3(camBox.xMax, camBox.yMin);
                                break;
                            default:
                                currentPos = new Vector3(camBox.xMin, camBox.yMin);
                                break;
                        }
                        Vector3 targetPos = target.transform.position;
                        Vector2 triangle = currentPos - targetPos;
                        float angle = Mathf.Atan2(-triangle.y, -triangle.x);
                        float fireAngle = UnityEngine.Random.Range(angle - (Mathf.Deg2Rad * machinegun.bulletSpread), angle + (Mathf.Deg2Rad * machinegun.bulletSpread));
                        Vector2 velocity = new Vector2(Mathf.Cos(fireAngle), Mathf.Sin(fireAngle));

                        GameObject proj = Instantiate(machinegun.machineGunProjectile);
                        proj.transform.position = currentPos;
                        BossProjectile bossProjectile;
                        proj.TryGetComponent<BossProjectile>(out bossProjectile);
                        if (bossProjectile)
                        {
                            bossProjectile.Setup(velocity, machinegun.machineGunDamage, machinegun.bulletSpeed, 5);
                        }
                    }
                    bulletCount++;
                }
            }
            shotTimer = 0;
        }
    }

    private void RandomPuddle()
    {
        if (puddleSpawnTimer > puddle.puddleSpawnTime)
        {
            if (puddleCount < puddle.spawnQuantity)
            {
                Rect camBox = TeaTime.cameraBoundingBox();
                Vector3 puddlePos = new Vector3(UnityEngine.Random.Range(camBox.xMin, camBox.xMax), UnityEngine.Random.Range(camBox.yMin, camBox.yMax), 0);
                GameObject temp = Instantiate(puddle.puddleObject, puddlePos, new Quaternion(0, 0, 0, 0));
                PuddleProjectile puddleProj = null;
                temp.TryGetComponent<PuddleProjectile>(out puddleProj);
                if (puddleProj == null)
                {
                    Debug.LogError("Puddle Projectile doesn't have the PuddleProjectile script");
                }
                puddleProj.Setup(puddle.puddleDamage, puddle.puddleAliveTime, puddle.puddleSizeChange, puddle.damageDelay, puddle.puddleSizeChangeAfterDelay);
                puddleCount++;
                puddleSpawnTimer = 0;
            }
            else
            {
                attackTimer = 0;
                isAttacking = false;
                return;
            }
        }
        else
        {
            puddleSpawnTimer += Time.deltaTime;
        }
    }
    private void PuddleOnPlayer()
    {
        if(puddleSpawnTimer > puddle.puddleSpawnTime)
        {
            if (puddleCount < puddle.spawnQuantity)
            {
                Vector3 targetPos = target.transform.position;
                GameObject temp = Instantiate(puddle.puddleObject, targetPos, new Quaternion());
                PuddleProjectile puddleProj = null;
                temp.TryGetComponent<PuddleProjectile>(out puddleProj);
                if (puddleProj == null)
                {
                    Debug.LogError("Puddle Projectile doesn't have the PuddleProjectile script");
                }
                puddleProj.Setup(puddle.puddleDamage, puddle.puddleAliveTime, puddle.puddleSizeChange, puddle.damageDelay, puddle.puddleSizeChangeAfterDelay);
                puddleCount++;
                puddleSpawnTimer = 0;
            }
            else
            {
                attackTimer = 0;
                isAttacking = false;
                return;
            }
        }
        else
        {
            puddleSpawnTimer += Time.deltaTime;
        }
    }

    private void PuddleOnBoss()
    {
        if (puddleSpawnTimer > puddle.puddleSpawnTime)
        {
            if (puddleCount < puddle.spawnQuantity)
            {
                Vector3 targetPos = gameObject.transform.position;
                GameObject temp = Instantiate(puddle.puddleObject, targetPos, new Quaternion());
                PuddleProjectile puddleProj = null;
                temp.TryGetComponent<PuddleProjectile>(out puddleProj);
                if (puddleProj == null)
                {
                    Debug.LogError("Puddle Projectile doesn't have the PuddleProjectile script");
                }
                puddleProj.Setup(puddle.puddleDamage, puddle.puddleAliveTime, puddle.puddleSizeChange, puddle.damageDelay, puddle.puddleSizeChangeAfterDelay);
                puddleCount++;
                puddleSpawnTimer = 0;
            }
            else
            {
                attackTimer = 0;
                isAttacking = false;
                return;
            }
        }
        else
        {
            puddleSpawnTimer += Time.deltaTime;
        }
    }

    private void Orbit()
    {
        if (!orbitSpawned)
        {
            float angleBetweenProj = ((2 * Mathf.PI) / orbit.projectileQuantity);
            for (int i = 0; i < orbit.projectileQuantity; i++)
            {
                GameObject temp = Instantiate(orbit.orbitObject, gameObject.transform);
                temp.transform.position = new Vector3(orbit.distanceFromBoss * Mathf.Cos(i * angleBetweenProj), orbit.distanceFromBoss * Mathf.Sin(i * angleBetweenProj))+gameObject.transform.position;
                OrbitProjectile orbitProj = null;
                temp.TryGetComponent<OrbitProjectile>(out orbitProj);
                if (orbitProj == null)
                {
                    Debug.LogError("Orbit Prefab missing OribitProjectile script");
                    return;
                }
                Debug.Log(temp.transform.position);
                orbitProj.Setup(orbit.orbitDamage, orbit.projectileTime, orbit.duration, i * angleBetweenProj, orbit.distanceFromBoss, orbit.revolutionChangeTime, orbit.finalRevolution, orbit.distanceChangeTime, orbit.timeTilChanges, orbit.doStatChanges);
            }
            orbitSpawned = true;
        }
        if(orbitDurationTimer >= orbit.duration)
        {
            isAttacking = false;
            attackTimer = 0;
            return;
        }
        orbitDurationTimer += Time.deltaTime;
    }

    public void Melee()
    {
        if(speedTimer >= melee.speed)
        {
            if(repeatCount < melee.numberRepeat)
            {
                if (meleeMovement == null) { }
                else
                {
                    meleeMovement.enabled = false;
                }
                meleeCollider.enabled = true;
                hitboxTimer = 0;
                speedTimer = 0;
                repeatCount++;
            }
            else
            {
                if (meleeMovement == null) { }
                else
                {
                    meleeMovement.enabled = false;
                }
                startMovement.enabled = true;
                Debug.Log("Finished Melee");
                isAttacking = false;
                attackTimer = 0;
                return;
            }
        }
        if(hitboxTimer >= melee.hitboxTime)
        {
            if (meleeMovement == null){}
            else
            {
                meleeMovement.enabled = true;
            }
            meleeCollider.enabled = false;
        }
        hitboxTimer += Time.deltaTime;
        speedTimer += Time.deltaTime;
    }

    public void EnemySpawn()
    {
        GameObject temp;
        Rect camBox = TeaTime.cameraBoundingBox();
        if (!spawnedEnemies)
        {
            switch (enemySpawn.enemySpawnLocation)
            {
                case BossEnemySpawnLocation.OffScreen:
                    for (int i = 0; i < enemySpawn.enemyQuantity; i++)
                    {
                        float randomNum = UnityEngine.Random.Range(0, 4);
                        if (randomNum <= 1)
                        {
                            Vector3 offScreenLoc = new Vector3(UnityEngine.Random.Range(camBox.xMin, camBox.xMax), camBox.yMin - 3);
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                        else if (randomNum <= 2)
                        {
                            Vector3 offScreenLoc = new Vector3(camBox.xMin - 3, UnityEngine.Random.Range(camBox.yMin, camBox.yMax));
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                        else if (randomNum <= 3)
                        {
                            Vector3 offScreenLoc = new Vector3(UnityEngine.Random.Range(camBox.xMin, camBox.xMax), camBox.yMax + 3);
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                        else
                        {
                            Vector3 offScreenLoc = new Vector3(camBox.xMax + 3, UnityEngine.Random.Range(camBox.yMin, camBox.yMax));
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                    }
                    break;
                case BossEnemySpawnLocation.OnScreen:
                    for (int i = 0; i < enemySpawn.enemyQuantity; i++)
                    {
                        Vector3 onScreenLoc = new Vector3(UnityEngine.Random.Range(camBox.xMin, camBox.xMax), UnityEngine.Random.Range(camBox.yMin, camBox.yMax), 0);
                        temp = Instantiate(enemySpawn.enemy, onScreenLoc, new Quaternion());
                    }
                    break;
                case BossEnemySpawnLocation.OnBoss:
                    for (int i = 0; i < enemySpawn.enemyQuantity; i++)
                    {
                        temp = Instantiate(enemySpawn.enemy, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.3f, 0.3f), transform.position.y + UnityEngine.Random.Range(-0.3f, 0.3f)), new Quaternion());
                    }
                    break;
                case BossEnemySpawnLocation.InCorner:
                    for (int i = 0; i < enemySpawn.enemyQuantity; i++)
                    {
                        float randomNum = UnityEngine.Random.Range(0, 4);
                        if (randomNum <= 1)
                        {
                            Vector3 offScreenLoc = new Vector3(camBox.xMin - 3, camBox.yMin - 3);
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                        else if (randomNum <= 2)
                        {
                            Vector3 offScreenLoc = new Vector3(camBox.xMin - 3, camBox.yMax + 3);
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                        else if (randomNum <= 3)
                        {
                            Vector3 offScreenLoc = new Vector3(camBox.xMax + 3, camBox.yMax + 3);
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                        else
                        {
                            Vector3 offScreenLoc = new Vector3(camBox.xMax + 3, camBox.yMin - 3);
                            temp = Instantiate(enemySpawn.enemy, offScreenLoc, new Quaternion());
                        }
                    }
                    break;
                case BossEnemySpawnLocation.None: break;
            }
            spawnedEnemies = true;
        }
        if(enemySpawn.additionalDelay <= additionalDelayTimer)
        {
            isAttacking = false;
            attackTimer = 0;
        }
        additionalDelayTimer += Time.deltaTime;
    }

    public void GetPlayerTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public void GetNearestObject()
    {
        
    }



}
