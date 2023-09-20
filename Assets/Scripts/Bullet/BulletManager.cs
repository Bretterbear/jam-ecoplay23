using Services;
using System;
using UnityEngine;

//BH   | Notes
//CONS | Making the name more generic & making this a broader level state instantiation device

/// <summary>
/// Class handles registration & activation of the PoolService plus some admin functions
/// </summary>
public class BulletManager : MonoBehaviour
{
    /// <summary>
    /// Sets up object pooling system
    /// </summary>
    private void Awake()
    {
        // Register & Initialize the pool service for use by all projectile shooters in level
        ServiceLocator.Instance.Register(new PoolService());
        ServiceLocator.Instance.Get<PoolService>().Init();

        // Adds top level pooling heirarchy bins to avoid distracting heirarchy chaff
        for (int i = 0; i < Enum.GetNames(typeof(ProjectileType)).Length; i++)
        {
            GameObject storageHolder = new GameObject($"ObjPool - {(ProjectileType) i}");
            ServiceLocator.Instance.Get<PoolService>().AddPoolHolder(storageHolder);
        }
    }

    /// <summary>
    /// Unregisters the service associated with it
    /// </summary>
    private void OnDestroy()
    {
        ServiceLocator.Instance.Unregister<PoolService>();
    }
}