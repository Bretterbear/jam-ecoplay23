using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This function initializes services as the game begins to setup.
/// </summary>
public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        //BH| ServiceLocator init has to happen before any other calls
        ServiceLocator.Initialize();

        //BH| Add any other services you want to register on startup here
    }
}
