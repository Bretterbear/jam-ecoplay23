using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocator : MonoBehaviour
{
    // --- Non-Serialized Variable Declarations --- //
    GameManagerService _linkGMService;

    void Start()
    {
        _linkGMService = ServiceLocator.Instance.Get<GameManagerService>();
    }

    // Update is called once per frame
    void Update()
    {
        // Wildly inefficient, runs every frame regardless of movement, gotta go fast.
        _linkGMService.recordPlayerLocation(this.transform.position);
    }
}
