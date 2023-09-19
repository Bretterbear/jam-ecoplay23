using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//BH   | Notes
//TODO | Clean this ugliness up esp once I do proper projectile base class inheritance for food & bullets

/// <summary>
/// Object pooling system that stores projectiles of bullet & foodstyle
/// </summary>
public class PoolService : IService
{
    // --- Private Variable Declarations --- //
    private List<GameObject>[] _objectPool;
    private List<GameObject> _poolStorageBins;
    private int numberOfBulletStyles;
    private int numberOfFoodStyles;

    /// <summary>
    /// Run just after registering the service in a level to make sure pools are ready to start holding
    /// </summary>
    public void Init()
    {
        numberOfBulletStyles = Enum.GetNames(typeof(BulletStyle)).Length;
        numberOfFoodStyles =  Enum.GetNames(typeof(FoodStyle)).Length;

        _objectPool = new List<GameObject>[numberOfBulletStyles + numberOfFoodStyles];
        _poolStorageBins = new List<GameObject>();

        for (int i = 0; i < numberOfBulletStyles + numberOfFoodStyles; i++)
        {
            _objectPool[i] = new List<GameObject>();
        }
    }

    /// <summary>
    /// Searches requested object pool for an unused bullet & returns it if possible. Else returns null
    /// NOTE - Function does not reset TTL
    /// </summary>
    /// <param name="style">must be any type within the BulletType enum</param>
    /// <returns>an activated & ready to use bullet if available or NULL</returns>
    public GameObject GetProjectileFromPoop(int styleInt)
    {
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
        if (obj.CompareTag("Bullet"))
        {
            Bullet bulletRef = obj.GetComponent<Bullet>();
            if (bulletRef != null)
            {
                int bulletStyleRef = (int)bulletRef.GetBulletStyle();
                //Debug.Log($"<color=cyan>Stored bullet in pool {bulletStyleRef}</color>");
                _objectPool[bulletStyleRef].Add(obj);
                obj.transform.SetParent(_poolStorageBins[bulletStyleRef].transform, false);
                return;
            }
        }
        else if (obj.CompareTag("Food"))
        {
            FoodItem foodRef = obj.GetComponent<FoodItem>();
            if (foodRef != null)
            {
                int foodStyleRef = (int)foodRef.GetFoodStyle();
                _objectPool[foodStyleRef].Add(obj);
                obj.transform.SetParent(_poolStorageBins[foodStyleRef].transform, false);
                //Debug.Log($"<color=cyan>Stored food in pool {foodStyleRef}</color>");
                return;
            }
        }
        Debug.LogError($"<color=orange>Bad use of Pool.AddToPool() - Attempted to add a non-pooled object {obj.name} into Pool</color>");
    }

    /// <summary>
    /// Remove a particular object from the pools
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveFromPool(GameObject obj)
    {
        if (obj.CompareTag("Bullet"))
        {
            Bullet bulletRef = obj.GetComponent<Bullet>();
            if (bulletRef != null)
            {
                int bulletStyleRef = (int)bulletRef.GetBulletStyle();
                _objectPool[bulletStyleRef].Remove(obj);
                return;
            }
        }
        else if (obj.CompareTag("Food"))
        {
            FoodItem foodRef = obj.GetComponent<FoodItem>();
            if (foodRef != null)
            {
                int foodStyleRef = (int)foodRef.GetFoodStyle();
                _objectPool[foodStyleRef].Remove(obj);
                return;
            }
        }
        Debug.LogError($"<color=orange>Bad use of Pool.RemoveFromPool() - {obj.name} is not a pooled object</color>");
        
    }

    public void AddPoolHolder(GameObject obj)
    {
        _poolStorageBins.Add(obj);
    }
}



