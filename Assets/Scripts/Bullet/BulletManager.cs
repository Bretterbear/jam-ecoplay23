using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BH| Notes
//TODO | Make Heirarchy Pool Holding locations work

/// <summary>
/// Class handles registration & activation of the BulletPoolService plus some admin functions
/// </summary>
public class BulletManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Instance.Register(new PoolService());
        ServiceLocator.Instance.Get<PoolService>().Init();

        for (int i = 0; i < Enum.GetNames(typeof(BulletStyle)).Length + Enum.GetNames(typeof(FoodStyle)).Length; i++)
        {
            GameObject storageHolder = new GameObject($"ObjPool-{i}");
            ServiceLocator.Instance.Get<PoolService>().AddPoolHolder(storageHolder);
        }
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.Unregister<PoolService>();
    }
}