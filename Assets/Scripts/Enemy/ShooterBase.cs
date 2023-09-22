using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBase : MonoBehaviour
{

    // --- Serialized Variable Declarations --- //
    [Header("Spawner Base Properties")]
    [Tooltip("Radius out from core of shooting position")]
    [SerializeField] protected float shootRadius = 0.5f;
    [Tooltip("count of ShootingPositions")]
    [SerializeField, Range(1, 36)] protected int shotCount = 2;
    [Tooltip("Shoot Countdown (in seconds")]
    [SerializeField, Range(0, 2)] protected float shotCoolDown;
    [Tooltip("Bullet Speed")]
    [SerializeField, Range(0.1f, 5)] protected float bulletSpeed = 2f;

    [Header("Prefab References")]
    [Tooltip("Bullet - add more please")]
    [SerializeField] protected GameObject resourceBullet;

    // --- Protected Variable Declarations --- //
    protected int maxShootPosCount = 36;        // Used for setting shoot pos array size
    protected float shootingTimer = 0;          // Contains current countdown to next shot
    protected float rotCenterAngle;             // Stores where the rotational center is located for targeting
    protected int shotCountStored;              // For triggering recalc in redistributes
    protected GameObject[] shootPosition;       // Stores shot positions for 
    protected float[] shootAngle;               // useful in all
    protected GameObject rotationalCenter;      // definitely in base
    protected PoolService _linkBulletPool;      // Service link to the PoolService
    protected Vector2 impulseVector;            // shot vector to be used by all
    protected ProjectileType _projectileType;   // carries projectile type for instant


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Link to the very heavily used bullet pool
        _linkBulletPool = ServiceLocator.Instance.Get<PoolService>();

        // Initialization of runtime settings
        _projectileType = resourceBullet.GetComponent<Projectile>().GetProjectileType();
        shotCountStored = shotCount;

        // Set rotational centerpoint for any targeting systems
        rotationalCenter = new GameObject("RotCenter");
        rotationalCenter.transform.SetParent(transform, false);

        // Set up shooting position array (gets reset on each position reset call)
        shootPosition = new GameObject[maxShootPosCount];
        for (int i = 0; i < maxShootPosCount; i++)
        {
            shootPosition[i] = new GameObject($"{this.name}_ShootPosition_{i}");
            shootPosition[i].transform.SetParent(rotationalCenter.transform, false);
        }

        impulseVector = Vector2.right;
        shootAngle = new float[maxShootPosCount];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Instantiates a new bullet and returns it; might be unnecessary as a func
    /// </summary>
    /// <param name="style">style of bullet to be instatiated</param>
    /// <returns>new bullet in pool</returns>
    protected virtual GameObject GetABullet()
    {
        GameObject newBullet = _linkBulletPool.GetProjectileFromPoop(_projectileType);
        if (newBullet == null)
        {
            newBullet = Instantiate(resourceBullet, transform);
        }

        return newBullet;
    }

    protected virtual void FireBullet(GameObject obj, int i)
    {
        float rot = shootAngle[i] + rotCenterAngle;

        SetProjectileProperties(obj, impulseVector, bulletSpeed, rot, 6f);
    }

    /// Set ballistic properties of projectile for 'firing'
    /// </summary>
    /// <param name="obj">object to be fired - must be projectile inheriting</param>
    /// <param name="vel">unit velocity vector - (generally [1,0])</param>
    /// <param name="spd">speed of projectile - positive float </param>
    /// <param name="rot">rotational float - in degrees</param>
    /// <param name="ttl">time to live in seconds - float</param>
    protected virtual void SetProjectileProperties(GameObject obj, Vector2 vel, float spd, float rot, float ttl)
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
