using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRay : ShooterBase
{
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
            FireBullet(GetABullet(), 0);
            shootingTimer = shotCoolDown;
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

        float shotAngle = rotCenterAngle;
        shootAngle[0] = angle;
        angle *= Mathf.Deg2Rad;

        shootPosition[0].transform.position = transform.position + new Vector3(Mathf.Cos(angle) * shootRadius, Mathf.Sin(angle) * shootRadius, 0f);
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
