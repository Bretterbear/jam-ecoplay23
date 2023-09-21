using Services;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BulletShooter : MonoBehaviour
{

    // --- Serialized Variable Declarations --- //
    [Header("Spawner Properties")]
    [Tooltip("Radius out from core of shooting position")]
    [SerializeField] private float shootRadius = 0.5f;
    [Tooltip("count of ShootingPositions")]
    [SerializeField,Range(1, 36)] private int shotCount = 2;
    [Tooltip("Shoot Countdown (in seconds")]
    [SerializeField, Range(0, 2)] private float shotCooldown;
    [Tooltip("Degrees of Turn per Second")]
    [SerializeField, Range(-1080,1080)] private float rotSpeed;
    [Tooltip("Bullet Speed")]
    [SerializeField, Range(0.1f, 5)] private float bulletSpeed;


    [Header("Prefab References")]
    [Tooltip("Bullet - add more please")]
    [SerializeField] private GameObject resourceBullet;

    // --- Private Variable Declarations --- //
    private int maxShootPosCount = 36;
    private float shootingTimer=0;                // Contains current countdown to next shot
    private GameObject[] shootPosition;
    private GameObject rotationalCenter;
    private PoolService _linkBulletPool;        // Service link to the PoolService
    private float[] shootAngle;                 
    private float rotCenterAngle;
    private Vector2 impulseVector;

    private Color[] colors;

    void Start()
    {
        // Link to the very heavily used bullet pool
        _linkBulletPool = ServiceLocator.Instance.Get<PoolService>();

        colors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.magenta,
        };

        // Initialization of runtime settings

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

        resetShootingPositions(shotCount);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCenter();

        shootingTimer -= Time.deltaTime;

        if (shootingTimer < 0f)
        {
            SpawnBullets();
            shootingTimer = shotCooldown;
            //RedistributeShootingPoints(shotCount);
        }
    }

    private void RotateCenter()
    {
        rotationalCenter.transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
        rotCenterAngle = rotationalCenter.transform.rotation.eulerAngles.z;
    }

    private void resetShootingPositions(int posCount)
    {
        rotationalCenter.transform.rotation = Quaternion.identity;
        rotCenterAngle = 0;

        RedistributeShootingPoints(posCount);
    }

    private void RedistributeShootingPoints(int posCount)
    {
        float rotOffset = 360f / posCount;

        for (int i = 0; i < posCount; i++)
        {
            float angle = rotOffset * i;
            shootAngle[i] = angle;
            angle *= Mathf.Deg2Rad;

            shootPosition[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle) * shootRadius, Mathf.Sin(angle) * shootRadius, 0f);
        }
    }

    public void SpawnBullets()
    {
        for (int i = 0; i < shotCount; i++)
        {
            GameObject newBullet = GetABullet();
            newBullet.transform.position = shootPosition[i].transform.position;
            FireBullet(newBullet, i);
        }

    }

    /// <summary>
    /// Instantiates a new bullet and returns it; might be unnecessary as a func
    /// </summary>
    /// <param name="style">style of bullet to be instatiated</param>
    /// <returns>new bullet in pool</returns>
    public GameObject GetABullet()
    {
        GameObject newBullet = _linkBulletPool.GetProjectileFromPoop(ProjectileType.Bullet_Ball);
        if (newBullet == null)
        {
            newBullet = Instantiate(resourceBullet, transform);
        }

        return newBullet;
    }

    public void FireBullet(GameObject obj, int i)
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

        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        //spriteRenderer.color = colors[(int) (Time.time*2) % (colors.Length)];

    }
}