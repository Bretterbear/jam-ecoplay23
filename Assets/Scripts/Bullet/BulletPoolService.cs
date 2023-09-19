using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BH   | Notes
//TODO | Need to work w/ BulletManager to set up pool holding locations (done in "addToPool" and undo the deparenting stuff in the spawner
public class BulletPoolService : IService
{
    // --- Private Variable Declarations --- //
    private List<GameObject>[] _objectPool;
    private int numberOfBulletStyles;

    /// <summary>
    /// Run just after registering the service in a level to make sure pools are ready to start holding
    /// </summary>
    public void Init()
    {
        numberOfBulletStyles = Enum.GetNames(typeof(BulletStyle)).Length;

        _objectPool = new List<GameObject>[numberOfBulletStyles];

        for (int i = 0; i < numberOfBulletStyles; i++)
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
    public GameObject GetBulletFromPoop(BulletStyle style)
    {
        for (int i = 0; i < _objectPool[(int) style].Count; i++)
        {
            if (!_objectPool[(int)style][i].activeSelf)
            {
                _objectPool[(int)style][i].SetActive(true);
                return _objectPool[(int)style][i];
            }
        }
        return null;
    }

    /// <summary>
    /// Add a bullet into the bullet pool
    /// </summary>
    /// <param name="obj"></param>
    public void AddToPool(GameObject obj)
    {
        Bullet bulletRef = obj.GetComponent<Bullet>();

        if (bulletRef != null )
        {
            int bulletStyleRef = (int)bulletRef.GetBulletStyle();
            _objectPool[bulletStyleRef].Add(obj);
        }
        else
        {
            Debug.LogError($"<color=orange>Bad use of BulletPool.AddToPool() - Attempted to add a non-bullet object {obj.name} into BulletPool</color>");
        }
    }

    /// <summary>
    /// Remove a particular bullet from the bullet pool
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveFromPool(GameObject obj)
    {
        Bullet bulletRef = obj.GetComponent<Bullet>();

        if (bulletRef != null)
        {
            int bulletStyleRef = (int)bulletRef.GetBulletStyle();
            _objectPool[bulletStyleRef].Add(obj);
        }
        else
        {
            Debug.LogError($"<color=orange>Bad use of BulletPool.RemoveFromPool() - {obj.name} is not a bullet</color>");
        }
    }
}
