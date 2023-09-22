using Services;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ShooterPlayerArc : ShooterBase
{
    // --- ShooterBase Serialized Variables --- //
    // shootRadius  |- Radius from core of shooting position (float)
    // shotCount    |- Count of shooting positions  (int)
    // shotCoolDown |- time between shot bursts in seconds (float)
    // bulletSpeed  |- Level of speed imparted to bullets (float)

    // --- ShooterBase Private Variable Declarations --- //
    //private int maxShootPosCount = 36;  // value might change by class
    //private float shootingTimer = 0;    // Contains current countdown to next shot

    //private float rotCenterAngle;   // stores where the rotational center is located for targeting
    //private int shotCountStored;    // for triggering recalc in redistributes

    //protected GameObject[] shootPosition;   // useful in all
    //private float[] shootAngle;             // useful in all

    //private GameObject rotationalCenter;    // definitely in base
    //private PoolService _linkBulletPool;    // Service link to the PoolService

    //private Vector2 impulseVector;          // shot vector to be used by all

    [Header("Spawner Properties")]
    [Tooltip("Firing arc in degrees")]
    [SerializeField, Min(0)] private float shootingArc;

    private Vector3 playerLocation;
    private GameManagerService _linkGMService;

    // Start is called before the first frame update
    protected override void Start()
    {
        _linkGMService = ServiceLocator.Instance.Get<GameManagerService>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        shootingTimer -= Time.deltaTime;

        if (shootingTimer < 0)
        {
            Targeting();
            for (int i = 0; i < shotCount; i++)
            {
                FireBullet(GetABullet(), i);
            }
            randomizeCooldown();
        }
    }

    private void Targeting()
    {
        playerLocation = _linkGMService.GetPlayerLocation();

        Vector3 directionToPlayer = playerLocation - transform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Create a rotation quaternion based on the angle
        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Smoothly rotate the enemy towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);

        //float shotAngle = rotCenterAngle;
        //shootAngle[0] = angle;
        //angle *= Mathf.Deg2Rad;

        //shootPosition[0].transform.position = transform.position + new Vector3(Mathf.Cos(angle) * shootRadius, Mathf.Sin(angle) * shootRadius, 0f);
        setShootPoints(directionToPlayer);
    }

    private void setShootPoints(Vector3 targetVector)
    {
        float degreeOffset = shootingArc / shotCount;

        float targetAngle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
        float angle = targetAngle;

        float shotAngle = rotCenterAngle;
        shootAngle[0] = angle;
        angle *= Mathf.Deg2Rad;
        shootPosition[0].transform.position = transform.position + new Vector3(Mathf.Cos(angle) * shootRadius, Mathf.Sin(angle) * shootRadius, 0f);

        int sign = -1;
        for (int i = 1; i < shotCount; i++)
        {
            angle = sign * degreeOffset * i + targetAngle;
            shootAngle[i] = angle;
            angle *= Mathf.Deg2Rad;
            shootPosition[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle) * shootRadius, Mathf.Sin(angle) * shootRadius, 0f);
            sign *= -1;
        }
    }

    protected override GameObject GetABullet()
    {
        return base.GetABullet();
    }

    protected override void FireBullet(GameObject obj, int i)
    {
        base.FireBullet(obj, i);
    }

    protected override void SetProjectileProperties(GameObject obj, Vector2 vel, float spd, float rot, float ttl)
    {
        obj.transform.position = shootPosition[0].transform.position;
        base.SetProjectileProperties(obj, vel, spd, rot, ttl);
    }
}
