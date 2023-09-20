using Services;
using UnityEngine;

//BH   | Notes
//TODO | 1 Extend pattern spawn to an enum w/ your spawn pattern presets (out of rando-vs-spawn)
//TODO | 2 Make a variable (Serial or pattern selectable) bullet type poolservice call
//TODO | 3 Off of 2, add some pattern / serial bullet info to add into initialization for overriding bullet base in MakeNewBullet()
//TODO | 4 Offload setting of bullet dynamics into a bullet local function

public class BulletSpawner : MonoBehaviour
{
    // --- Serialized Variable Declarations --- //
    [Header("Spawner Properties")]
    [Tooltip("Minimum firing arc in degrees")]
    [SerializeField] private float minRotation;
    [Tooltip("Maximum firing arc in degrees")]
    [SerializeField] private float maxRotation;
    [Tooltip("Bullet count per shot")]
    [SerializeField] private int bulletsPerCycle;
    [Tooltip("Shoot equidistant or at random")]
    [SerializeField] private bool bPatternSpawning;                 // Likely move to random mode once we have pattern
    [Tooltip("Seconds between shots")]
    [SerializeField] private float shootingCooldown= 0.5f;          // Default val not a bad idea
    [Tooltip("Initial bullet velocity")]
    [SerializeField] private float bulletSpeed;                     // Likely end up as part of a SerializedObject
    [Tooltip("Unit Vector please - controls bullet orientation")]
    [SerializeField] private Vector2 bulletVelocity;                // Likely to remove this

    [Header("Prefab References")]
    [Tooltip("Bullet - add more please")]
    [SerializeField] private GameObject resourceBullet;

    // --- Private Variable Declarations --- //
    private float shootingTimer;                // Contains current countdown to next shot
    private float[] rotations;                  // Contains firing rotations for shot burst
    private PoolService _linkBulletPool;        // Service link to the PoolService

    /// <summary>
    /// Initial set up of pooling link, shooting positions & timer
    /// </summary>
    void Start()
    {
        _linkBulletPool = ServiceLocator.Instance.Get<PoolService>();

        shootingTimer = shootingCooldown;

        rotations = new float[bulletsPerCycle];
        if (bPatternSpawning)
        {
            RotationsDistributed();
        }

    }

    /// <summary>
    /// Shot clock & spawn calls
    /// </summary>
    void Update()
    {
        if (shootingTimer <= 0)
        {
            SpawnBullets();
            shootingTimer = shootingCooldown;
        }
        shootingTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Sets random shooting locations within the allowabla arc
    /// </summary>
    /// <returns></returns>
    public float[] RotationsRandom()
    {
        for (int i = 0; i < bulletsPerCycle; i++)
        {
            rotations[i] = UnityEngine.Random.Range(minRotation, maxRotation);
        }

        return rotations;
    }

    /// <summary>
    /// Defines an even number of bullet spawn points within minRotations -> maxRotations arc
    /// return value is useless, but we're changing other stuff anyways
    /// </summary>
    public float[] RotationsDistributed()
    {
        for (int i = 0; i < bulletsPerCycle; i++)
        {
            rotations[i] = ((float) i / (bulletsPerCycle - 1f))     //BH| How fractionally do we need to divide
                            * (maxRotation - minRotation)           //BH| Multiply by our valid rotational arc
                            + minRotation;                          //BH| Set min val to the start of our valid rotational arc
        }
        return rotations;
    }

    /// <summary>
    /// Bullet spawning function, will get much more sophisticated w/ time
    /// </summary>
    /// <returns></returns>
    public GameObject[] SpawnBullets()
    {
        //BH| Switch up the pattern if we're in random-spawn mode
        if (!bPatternSpawning)
        {
            RotationsRandom();
        }

        GameObject[] spawnedBullets = new GameObject[bulletsPerCycle];
        for (int i = 0; i < bulletsPerCycle; i++)
        {
            //For each requisite
            spawnedBullets[i] = _linkBulletPool.GetProjectileFromPoop(ProjectileType.Bullet_Poke);
            if (spawnedBullets[i] == null)
            {
                spawnedBullets[i] = MakeNewBullet(ProjectileType.Bullet_Poke);
            }
            // else { Debug.Log($"<color=blue>We're spitting {spawnedBullets[i].name}</color>"); }

            spawnedBullets[i].transform.position = transform.position;
            SetProjectileProperties(spawnedBullets[i], bulletVelocity, bulletSpeed, rotations[i], 6f);
        }

        return spawnedBullets;

    }

    /// <summary>
    /// Instantiates a new bullet and returns it; might be unnecessary as a func
    /// </summary>
    /// <param name="style">style of bullet to be instatiated</param>
    /// <returns>new bullet in pool</returns>
    public GameObject MakeNewBullet(ProjectileType style)
    {
        // As we add bullet types, we'll have the spawner have an array of prefab bullet types
        GameObject newBullet = Instantiate(resourceBullet, transform);

        return newBullet;
    }

    /// <summary>
    /// Sets physical & engine properties of bullet prior to "firing"
    /// </summary>
    public void SetBulletProperties(Bullet bullet, int i)
    {
        bullet.transform.position = transform.position;

        bullet.timeToLive = 6;              //BH| For now this is a magic number, will be proper later
        bullet.rotation = rotations[i];
        bullet.speed = bulletSpeed;
        bullet.velocity = bulletVelocity;

        bullet.tag = "Bullet";
    }

    /// <summary>
    /// Set ballistic properties of projectile for 'firing'
    /// </summary>
    /// <param name="obj">object to be fired - must be projectile inheriting</param>
    /// <param name="vel">unit velocity vector - (generally [1,0])</param>
    /// <param name="spd">speed of projectile - positive float </param>
    /// <param name="rot">rotational float - in degrees</param>
    /// <param name="ttl">time to live in seconds - float</param>
    public void SetProjectileProperties(GameObject obj, Vector2 vel, float spd, float rot, float ttl)
    {
        Projectile projectile = obj.GetComponent<Projectile>();
        if (projectile == null)
        {
            Debug.LogError($"<color=orange>ERROR - {obj.name} not a fireable projectile </color>");
        }

        projectile.velocity = vel;
        projectile.rotation = rot;
        projectile.speed = spd;
        projectile.timeToLive = ttl;
    }
}
