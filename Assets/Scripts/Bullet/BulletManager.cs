using Services;
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
    [Header("Pool Holding Locations in Heirarchy")]
    [Tooltip("Game Object References")]
    public string beatmap;

    private void Awake()
    {
        ServiceLocator.Instance.Register(new BulletPoolService());
        ServiceLocator.Instance.Get<BulletPoolService>().Init();
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.Unregister<BulletPoolService>();
    }
}