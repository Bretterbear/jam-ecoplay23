using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//BH| Notes
//TODO - Extend pattern spawn to an enum w/ your spawn pattern presets (out of rando-vs-spawn)
//TODO - Migrate bullet spawning into proper object pooling to load limit

public class BulletSpawner : MonoBehaviour
{
    public GameObject resourceBullet;
    public float minRotation;
    public float maxRotation;
    public int bulletsPerCycle;
    public bool bPatternSpawning;

    public float cooldown;
    public float bulletSpeed;
    public Vector2 bulletVelocity;

    float timer;
    float[] rotations;

    private int bulletCounter = 0;



    // Start is called before the first frame update
    void Start()
    {
        timer = cooldown;
        bulletCounter = 0;

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
            spawnedBullets[i] = BulletManager.GetBulletFromPoop();
            if (spawnedBullets[i] == null)
            {
                spawnedBullets[i] = Instantiate(resourceBullet, transform);
                BulletManager.bulletsInUse.Add(spawnedBullets[i]);
                bulletCounter++;
                Debug.Log("bullet count: " + bulletCounter);
            }
            else
            {
                spawnedBullets[i].transform.SetParent(transform);
                spawnedBullets[i].transform.localPosition = Vector2.zero;
            }
            Bullet bulletRef = spawnedBullets[i].GetComponent<Bullet>();
            bulletRef.rotation = rotations[i];
            bulletRef.speed = bulletSpeed;
            bulletRef.velocity = bulletVelocity;
            bulletRef.tag = "Bullet";
        }

        return spawnedBullets;

    }
}
