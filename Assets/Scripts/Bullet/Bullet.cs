using UnityEngine;

//BH   | Notes
//CONS | Add grabbing child transform to start func & make a func to jiggle or alter child sprite component for VFX / effect

/// <summary> 
/// Contains Bullet behavior as inherited from Projectile 
/// </summary>
public class Bullet : Projectile
{
    // --- Serialized Variable Declarations --- //
    [Header("Bullet Properties")]
    [Tooltip("Bullet damage value (default 1f)")]
    [SerializeField] private float damageValue = 1f;

    /// <summary> 
    /// Sets parent to null & adds projectile into the PoolService 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary> 
    /// Inheriting from Projectile base class to automatically carry out motion & TTL deactivation
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    /// <summary> 
    /// Should never need to destroy a pooled object mid-level, but if you do, will try to unpool itself before it dies 
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// Used to enure the game manager has an accurate bullet density readinng
    /// </summary>
    private void OnDisable()
    {
        _linkGMService.UnregisterBullet();
    }

    /// <summary>
    /// Used to enure the game manager has an accurate bullet density readinng
    /// </summary>
    private void OnEnable()
    {
        _linkGMService.RegisterBullet();
    }

    /// <summary>
    /// Getter function inherited from Projectile base class
    /// </summary>
    /// <returns>Enum ProjectileType value assigned to object</returns>
    public override ProjectileType GetProjectileType()
    {
        return base.GetProjectileType();
    }
}