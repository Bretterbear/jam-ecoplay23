using Services;
using System;
using UnityEngine;

//BH   | Notes
//TODO | Should move all the variable setting from the Spawner to a func in here & make them private

/// <summary>
/// Projectile base class to be inherited by food & bullets 
/// </summary>
public class Projectile : MonoBehaviour
{
    // --- Serialized Variable Declarations --- //
    [Header("Projectile Properties")]
    [Tooltip("Projectile type (Bullet_* / Food_*)")]
    [SerializeField] private ProjectileType type;
    [Tooltip("Projectile value - will be added, set to negative for subtraction (default 1f)")]
    [SerializeField] private float value = 1f;

    // --- Non-Serialized Variable Declarations --- //
    [NonSerialized] public Vector2 velocity;            // Unit vector modified by speed to give directionality (generally [1,0])
    [NonSerialized] public float speed;                 // Set by whatever spawner fires it, may change over time
    [NonSerialized] public float rotation;              // Set by whatever spawner fires it, may change over time
    [NonSerialized] public float timeToLive = 6f;       // Set by spawner, needs to be long enough to not "blip" out on screen

    protected GameManagerService _linkGMService;

    /// <summary> 
    /// Sets parent to null & adds projectile into the PoolService 
    /// </summary>
    protected virtual void Awake()
    {
        // Precautionary, parent should be being reset to a poolBin when added to the pool anyways
        transform.SetParent(null);

        // As soon as object comes to life, needs to be added to an object pool
        ServiceLocator.Instance.Get<PoolService>().AddToPool(this.gameObject);

        _linkGMService = ServiceLocator.Instance.Get<GameManagerService>();
    }

    /// <summary> 
    /// Handles motion & TTL for projectile features 
    /// </summary>
    protected virtual void Update()
    {

        timeToLive -= Time.deltaTime;
        if (timeToLive < 0)
        {
            gameObject.SetActive(false);
        }
        MoveProjectile();
    }

    /// <summary> 
    /// Should never need to destroy a pooled object mid-level, but if you do, will try to unpool itself before it dies 
    /// </summary>
    protected virtual void OnDestroy()
    {
        // If this is a deletion in-play, then shout to the object pool "take me OOOOUUUT!"
        if (gameObject.scene.isLoaded)
        {
            ServiceLocator.Instance.Get<PoolService>().RemoveFromPool(this.gameObject);
        }
    }

    /// <summary>
    /// Simple for now, will have increasing complexity over time
    /// </summary>
    protected virtual void MoveProjectile()
    {
        if (transform.rotation.eulerAngles.z != rotation)
        {
            this.transform.rotation = Quaternion.Euler(0,0,rotation);
        }
        transform.Translate(velocity * speed * Time.deltaTime);
    }

    public virtual float GetProjectileValue()
    {
        return value;
    }

    /// <summary>
    /// Returns type of projectile that bullet is set to
    /// </summary>
    /// <returns></returns>
    public virtual ProjectileType GetProjectileType()
    {
        return type;
    }
}

/// <summary>
/// All pooled projectile types in the game (both bullets & food
/// </summary>
public enum ProjectileType
{
    Bullet_Poke = 0,
    Bullet_Ball = 1,
    Food_Seaweed = 2
}