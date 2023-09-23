using Services;
using UnityEngine;

public class ShooterSpin : ShooterBase
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

    // --- Serialized Variable Declarations --- //
    [Tooltip("Degrees of Turn per Second")]
    [SerializeField, Range(-1080, 1080)] private float rotSpeed;

    protected override void Start()
    {
        //any calls to lower max or change init vars must be here
        base.Start();

        shotCountStored = shotCount;

        resetShootingPositions(shotCount);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCenter();

        shootingTimer -= Time.deltaTime;

        if (shootingTimer < 0f)
        {
            randomizeCooldown();

            if (shotCount != shotCountStored)
            {
                //shotCountStored = shotCount;
                //RedistributeShootingPoints(shotCount);
            }

            SpawnBullets();
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
            float angle = rotOffset * i + rotCenterAngle;
            shootAngle[i] = angle;
            angle *= Mathf.Deg2Rad;

            shootPosition[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle) * shootRadius, Mathf.Sin(angle) * shootRadius, 0f);
        }
    }

    protected void SpawnBullets()
    {
        for (int i = 0; i < shotCount; i++)
        {
            GameObject newBullet = GetABullet();
            newBullet.transform.position = shootPosition[i].transform.position;
            FireBullet(newBullet, i);
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Shoot2");
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
        base.SetProjectileProperties(obj, vel, spd, rot, ttl);
    }

}