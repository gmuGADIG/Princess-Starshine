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

public enum BossEnemySpawnLocation
{
    OffScreen,
    OnScreen,
    OnBoss,
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
    public Vector2 projectileOffset;
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
    [Tooltip("Enable if the puddles should spawn ")]
    public bool spawnOnPlayer;
    public int spawnQuantity;
    public float puddleSizeChange;
    public bool puddleSizeChangeAfterDelay;
    public float damageDelay;
    public GameObject puddleObject;
}

[Serializable]
public struct Orbit
{
    public float orbitDamage;
    [Tooltip("The amount of time the projectile takes to go around the circle")]
    public float projectileTime;
    public int projectileQuantity;
    public float distanceFromBoss;
    public float distanceChangeTime;
    [Tooltip("The change in time for a revolution of the orbit, this is the final time that the projectiles will go to")]
    public float finalRevolution;
    public float revolutionChangeTime;
    public bool doStatChanges;
    public float timeTilChanges;
    public float duration;
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
    public int movementType;
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
    private float speedTimer;
    private float hitboxTimer;
    private int repeatCount;
    private CircleCollider2D meleeCollider;


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
   


    void Start()
    {
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
        TryGetComponent<CircleCollider2D>(out meleeCollider);
        meleeCollider.enabled = false;
        meleeCollider.radius = melee.radius;
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
                spawnedEnemies = false;
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

    private void Orbit()
    {
        if (!orbitSpawned)
        {
            float angleBetweenProj = ((2 * Mathf.PI) / orbit.projectileQuantity);
            for (int i = 0; i < orbit.projectileQuantity; i++)
            {
                GameObject temp = Instantiate(orbit.orbitObject, gameObject.transform.position, new Quaternion());
                temp.transform.position += new Vector3(orbit.distanceFromBoss * Mathf.Cos(i * angleBetweenProj), orbit.distanceFromBoss * Mathf.Sin(i * angleBetweenProj));
                OrbitProjectile orbitProj = null;
                temp.TryGetComponent<OrbitProjectile>(out orbitProj);
                if (orbitProj == null)
                {
                    Debug.LogError("Orbit Prefab missing OribitProjectile script");
                    return;
                }
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
                meleeCollider.enabled = true;
                hitboxTimer = 0;
                speedTimer = 0;
                repeatCount++;
            }
            else
            {
                isAttacking = false;
                attackTimer = 0;
            }
        }
        if(hitboxTimer >= melee.hitboxTime)
        {
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
