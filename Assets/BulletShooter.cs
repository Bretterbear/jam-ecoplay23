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
    [SerializeField] private int initShootPosCount = 2;
    [Tooltip("Max number of ShootingPositions")]
    [SerializeField] private int maxShootPosCount = 12;
    [Tooltip("rotTime")]
    [SerializeField] float rotTime = 6f;

    [Header("Prefab References")]
    [Tooltip("Bullet - add more please")]
    [SerializeField] private GameObject resourceBullet;

    // --- Private Variable Declarations --- //
    private float shootingTimer;                // Contains current countdown to next shot
    private GameObject[] shootPosition;
    private GameObject rotationalCenter;
    private PoolService _linkBulletPool;        // Service link to the PoolService
    private float[] shootAngle;
    private float rotCenterAngle;
    private float rotSpeed;
    private Vector2 impulseVector;

    private Color[] colors;

    void Start()
    {
        colors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.magenta,
        };

        shootingTimer = 1f;

        _linkBulletPool = ServiceLocator.Instance.Get<PoolService>();

        impulseVector = Vector2.up;

        shootPosition = new GameObject[maxShootPosCount];
        shootAngle = new float[maxShootPosCount];

        rotationalCenter = new GameObject("RotCenter");
        rotationalCenter.transform.SetParent(transform, false);

        rotSpeed = 360f / rotTime;

        for (int i = 0; i < maxShootPosCount; i++)
        {
            shootPosition[i] = new GameObject($"{this.name}_ShootPosition_{i}");
            shootPosition[i].transform.SetParent(rotationalCenter.transform, false);
        }
        resetShootingPositions(initShootPosCount);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCenter();

        shootingTimer -= Time.deltaTime;

        if (shootingTimer < 0f)
        {
            SpawnBullets();
            shootingTimer = .1f;
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
        for (int i = 0; i < initShootPosCount; i++)
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

        SetProjectileProperties(obj, impulseVector, 3f, rot, 6f);


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

        spriteRenderer.color = colors[(int) (Time.time*2) % (colors.Length)];

    }
}