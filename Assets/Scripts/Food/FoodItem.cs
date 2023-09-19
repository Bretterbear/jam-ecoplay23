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

    // --- Non-Serialized Variable Declarations --- //
    [NonSerialized] public Vector2 velocity;            // Set by FoodSpawner) that fires it
    [NonSerialized] public float speed = 1f;            // BH|CON Consider changing, will become variable overtime for certain bullet patterns
    [NonSerialized] public float rotation;              // Set by FSS that fires it
    [NonSerialized] public float timeToLive = 6f;       // Set by FS

    private void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0f)
        {
            Destroy(gameObject);
        }
        MoveFood();
    }

    public float GetFoodValue()
    {
        return foodValue;
    }

    void MoveFood()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
    }
}
