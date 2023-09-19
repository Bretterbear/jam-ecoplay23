using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//BH   | Notes
//TODO | 1 Extend pattern spawn to an enum w/ your spawn pattern presets (out of rando-vs-spawn)
//TODO | 2 Make a variable (Serial or pattern selectable) bullet type poolservice call
//TODO | 3 Off of 2, add some pattern / serial bullet info to add into initialization for overriding bullet base in MakeNewBullet()
//TODO | 4 Offload setting of bullet dynamics into a bullet local function

public class BulletSpawner : MonoBehaviour
{
    public GameObject resourceBullet;
    public float minRotation;
    public float maxRotation;
    public int bulletsPerCycle;
    public bool bPatternSpawning;
    public bool bIsParent;

    public float cooldown;
    public float bulletSpeed;
    public Vector2 bulletVelocity;

    float timer;
    float[] rotations;

    // --- Private Variable Declarations --- //
    private BulletPoolService _linkBulletPool;      //Service Link to the bulletPool



    // Start is called before the first frame update
    void Start()
    {
        _linkBulletPool = ServiceLocator.Instance.Get<BulletPoolService>();

        timer = cooldown;

        rotations = new float[bulletsPerCycle];
        if (bPatternSpawning)
        {
            RotationsDistributed();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            SpawnBullets();
            timer = cooldown;
        }
        timer -= Time.deltaTime;
    }
    public float[] RotationsRandom()
    {
        for (int i = 0; i < bulletsPerCycle; i++)
        {
            rotations[i] = UnityEngine.Random.Range(minRotation, maxRotation);
        }

        return rotations;
    }

    /// <summary>
    ///BH| Defines an even number of bullet spawn points within minRotations -> maxRotations arc
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
            spawnedBullets[i] = _linkBulletPool.GetBulletFromPoop(BulletStyle.Poke);
            if (spawnedBullets[i] == null)
            {
                spawnedBullets[i] = MakeNewBullet(BulletStyle.Poke);
            }

            SetBulletProperties(spawnedBullets[i].GetComponent<Bullet>(), i);
        }

        return spawnedBullets;

    }

    /// <summary>
    /// Instantiates a new bullet and returns it; might be unnecessary as a func
    /// </summary>
    /// <param name="style">style of bullet to be instatiated</param>
    /// <returns>new bullet in pool</returns>
    public GameObject MakeNewBullet(BulletStyle style)
    {
        GameObject newBullet = Instantiate(resourceBullet, transform);

        return newBullet;
    }

    /// <summary>
    /// Sets physical & engine properties of bullet prior to "firing"
    /// </summary>
    public void SetBulletProperties(Bullet bullet, int i)
    {
        bullet.transform.SetParent(transform);
        bullet.transform.localPosition = Vector2.zero;

        if (!bIsParent)
        {
            bullet.transform.SetParent(null);
        }

        bullet.timeToLive = 6;              //BH| For now this is a magic number, will be proper later
        bullet.rotation = rotations[i];
        bullet.speed = bulletSpeed;
        bullet.velocity = bulletVelocity;

        bullet.tag = "Bullet";
    }
}
