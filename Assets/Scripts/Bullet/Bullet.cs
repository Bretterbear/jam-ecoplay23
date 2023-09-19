using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

//BH   | Notes
//TODO | Should move all the variable setting from the Spawner to a func in here & make them private
//TODO | Add grabbing child transform to start func & make a func to jiggle or alter child sprite component for VFX / effect

/// <summary>
/// Contains bullet behavior - WARNING; objects w/ bullet script should only be made by BulletSpawner class
/// </summary>
public class Bullet : MonoBehaviour
{
    // --- Public Variable Declarations --- //
    [NonSerialized] public Vector2 velocity;            // Set by BulletSpawner (BS) that fires it
    [NonSerialized] public float speed = 1f;            // BH|CON Consider changing, will become variable overtime for certain bullet patterns
    [NonSerialized] public float rotation;              // Set by BS that fires it
    [NonSerialized] public float timeToLive = 2f;       // Set by BS (if not set, will get enabled then redisabled almost instantly)

    // --- Private Variable Declarations --- //
    [Header("Bullet Style / Type")]
    [Tooltip("Type of Bullet - for Pooling Purposes")]
    [SerializeField] private BulletStyle bulletType = BulletStyle.Poke;

    // Start is called before the first frame update
    void Start()
    {
        // As soon as bullet starts up, gets dumked into the object pool
        ServiceLocator.Instance.Get<BulletPoolService>().AddToPool(this.gameObject);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    /// <summary>
    /// You shouldn't need to destroy a bullet in-level - but if you do, it'll at least get dropped from its pool
    /// </summary>
    private void OnDestroy()
    {
        // If this is a deletion in-play, then shout to the bullet pool "take me OOOOUUUT!"
        if (gameObject.scene.isLoaded)
        {
            ServiceLocator.Instance.Get<BulletPoolService>().RemoveFromPool(this.gameObject);
        }
    }

    void Update()
    {
        MoveBullet();
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Migrated into separate function as the movement is likely to get significantly more complicated
    /// </summary>
    void MoveBullet()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
    }

    public BulletStyle GetBulletStyle()
    {
        return bulletType;
    }
}

/// <summary>
/// Every bullet style should have an enum associated w/ it for pooling & behavior management
/// </summary>
public enum BulletStyle
{
    Poke,
    Ball
}