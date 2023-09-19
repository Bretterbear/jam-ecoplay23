using Services;
using System;
using System.Collections.Generic;
using UnityEngine;

//BH   | Notes
//CONS | Might swap out pooling List for a stack, currently get operation is O(n) and that's just sad
//CONS | Maybe adding an overhead bounding system. if avail pool of an obj is too large for too long we reduce its size?
//CONS | Adding a prequeuing system where we can request pool resources allocated before we need them at low priority?

/// <summary>
/// Object pooling system that stores projectiles of bullets & foods 
/// </summary>
public class PoolService : IService
{
    // --- Private Variable Declarations --- //
    private List<GameObject>[] _objectPool;         // Actual pooling storage, 1 per style
    private List<GameObject> _poolStorageBins;      // Instantiated holders for pooled objects in heirarchy
    private int countProjectileStyles;              // Currently barely used, might eliminate

    /// <summary>
    /// Run just after registering the service in a level to make sure pools are ready to start holding
    /// </summary>
    public void Init()
    {
        countProjectileStyles = Enum.GetNames(typeof(ProjectileType)).Length;

        _objectPool = new List<GameObject>[countProjectileStyles];
        _poolStorageBins = new List<GameObject>();

        for (int i = 0; i < countProjectileStyles; i++)
        {
            _objectPool[i] = new List<GameObject>();
        }
    }

    /// <summary>
    /// Searches requested object pool for an unused bullet & returns it if possible. Else returns null
    /// NOTE - Function does not reset TTL
    /// </summary>
    /// <param name="style">must be any type within the ProjectileType enum</param>
    /// <returns>an activated & ready to use bullet if available or NULL</returns>
    public GameObject GetProjectileFromPoop(ProjectileType style)
    {
        int styleInt = (int)style;
        for (int i = 0; i < _objectPool[styleInt].Count; i++)
        {
            if (!_objectPool[styleInt][i].activeSelf)
            {
                _objectPool[styleInt][i].SetActive(true);
                //Debug.Log($"<color=yellow>Returning {_objectPool[styleInt][i].name} from pool {styleInt}</color>");
                return _objectPool[styleInt][i];
            }
        }
        return null;
    }

    /// <summary>
    /// Add an object into the projectile pool
    /// </summary>
    /// <param name="obj"></param>
    public void AddToPool(GameObject obj)
    {
        Projectile projectile = obj.GetComponent<Projectile>();
        if (projectile != null)
        {
            int projectileStyleRef = (int)projectile.GetProjectileType();
            _objectPool[projectileStyleRef].Add(obj);
            //Debug.Log($"<color=cyan>Stored {obj.name} in pool {(ProjectileType) projectileStyleRef}</color>");
            obj.transform.SetParent(_poolStorageBins[projectileStyleRef].transform, false);
        }
        else
        {
            Debug.LogError($"<color=orange>Bad use of Pool.AddToPool() - Attempted to add a non-pooled object {obj.name} into Pool</color>");
        }
    }

    /// <summary>
    /// Remove a particular object from the pools
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveFromPool(GameObject obj)
    {
        Projectile projectile = obj.GetComponent<Projectile>();
        if (projectile != null)
        {
            int projectileStyleRef = (int)projectile.GetProjectileType();
            _objectPool[projectileStyleRef].Remove(obj);
        }
        else
        {
            Debug.LogError($"<color=orange>Bad use of Pool.RemoveFromPool() - {obj.name} is not a pooled object</color>");
        }
    }

    public void AddPoolHolder(GameObject obj)
    {
        _poolStorageBins.Add(obj);
    }
}



