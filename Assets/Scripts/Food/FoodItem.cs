using UnityEngine;

//BH    | Notes
//CONS  | Add grabbing child transform to star func to allow for VFX / sprite alteration on the fly 

/// <summary>
/// Contains Food behavior inherited from Projectile
/// </summary>
public class FoodItem : Projectile
{
    // --- Serialized Variable Declarations --- //
    [Header("Food Properties")] 
    [Tooltip("Food nutrition value (default to 1")]
    [SerializeField] private float foodValue = 1f;

    /// <summary> 
    /// Sets parent to null & adds projectile into the PoolService 
    /// </summary>
    protected override void Start()
    {
        base.Start();
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
    /// Getter function inherited from Projectile base class
    /// </summary>
    /// <returns>Enum ProjectileType value assigned to object</returns>
    public override ProjectileType GetProjectileType()
    {
        return base.GetProjectileType();
    }
}
