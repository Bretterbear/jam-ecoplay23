using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterArc : ShooterBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
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
