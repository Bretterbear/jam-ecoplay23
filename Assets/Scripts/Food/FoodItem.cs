using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BH| Note - should shift food item & bullet basic behavior to an inheritable base class
public class FoodItem : MonoBehaviour
{

    // --- Serlialized Variable Declarations --- //
    [Header("Food Properties")] 
    [Tooltip("Food nutrition value (default to 1")]
    [SerializeField] private float foodValue = 1f;
    [Tooltip("Food Style (enum)")]
    [SerializeField] public FoodStyle foodType;

    // --- Non-Serialized Variable Declarations --- //
    [NonSerialized] public Vector2 velocity;            // Set by FoodSpawner) that fires it
    [NonSerialized] public float speed = 1f;            // BH|CON Consider changing, will become variable overtime for certain bullet patterns
    [NonSerialized] public float rotation;              // Set by FSS that fires it
    [NonSerialized] public float timeToLive = 6f;       // Set by FS

    void Start()
    {
        transform.SetParent(null);
        foodType = new FoodStyle();
        foodType = FoodStyle.Seaweed;
        //Debug.Log($"<color=yellow>I'M FOOD & FOOD NUMBER IS {foodType}</color>");
        // As soon as bullet starts up, gets dumked into the object pool
        ServiceLocator.Instance.Get<PoolService>().AddToPool(this.gameObject);
    }
    private void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0f)
        {
            gameObject.SetActive(false);
        }
        MoveFood();
    }

    /// <summary>
    /// You shouldn't need to destroy food in-level - but if you do, it'll at least get dropped from its pool
    /// </summary>
    private void OnDestroy()
    {
        // If this is a deletion in-play, then shout to the object pool "take me OOOOUUUT!"
        if (gameObject.scene.isLoaded)
        {
            ServiceLocator.Instance.Get<PoolService>().RemoveFromPool(this.gameObject);
        }
    }

    public float GetFoodValue()
    {
        return foodValue;
    }

    void MoveFood()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
    }

    public FoodStyle GetFoodStyle()
    {
        return foodType;
    }
}

/// <summary>
/// Every bullet style should have an enum associated w/ it for pooling & behavior management
/// </summary>
public enum FoodStyle
{
    Seaweed=2,
}
