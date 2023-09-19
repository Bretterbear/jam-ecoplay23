using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEmitter : MonoBehaviour
{
    // --- Serialized Variable Declarations --- //
    [Header("Food Spawn Properties")]
    [Tooltip("Minimum cooldown time between food fires")]
    [SerializeField,MinAttribute(0.1f)] private float _coolDownMin=.1f;
    [Tooltip("Minimum cooldown time between food fires")]
    [SerializeField, MinAttribute(0.1f)] private float _coolDownMax=1f;
    [Tooltip("Measurement of x-distance from center we can point at")]
    [SerializeField, MinAttribute(0.1f)] private float _xSpread = 4.5f;
    [Tooltip("Measurement of y-distance from center we can point at")]
    [SerializeField, MinAttribute(0.1f)] private float _ySpread = 3.5f;

    [Header("Food Spawn Properties")]
    [Tooltip("Stores food object prefab")]
    [SerializeField] private GameObject FoodSource;
    [Tooltip("Stores offscreen spawn locales")]
    [SerializeField] private GameObject[] FoodSpawnPoints;

    // --- Non-Serialized Variable Declarations --- //
    private float spawnTimer = 1f;
    private int controlPointCount = 0;

    private void Start()
    {
        // Bounds checking nothing derpy was put into the cooldown min & max values
        _coolDownMax = _coolDownMin > _coolDownMax ? _coolDownMin : _coolDownMax;

        // Check how many control points we've got to shuffle between
        controlPointCount = FoodSpawnPoints.Length;

    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            Debug.Log("<color=cyan>FIRING FOOD</color>");
            ResetTimer();
            FireFood();
        }
    }

    private void FireFood()
    {
        int fireIndex = Random.Range(0, controlPointCount);

        float xRand = Random.Range(-_xSpread, _xSpread);
        float yRand = Random.Range(-_ySpread, _ySpread);
        Vector3 targetAngle = new Vector3(xRand, yRand, 0);

        targetAngle = (targetAngle - FoodSpawnPoints[fireIndex].transform.position).normalized;

        FoodItem foodInstance = Instantiate(FoodSource, FoodSpawnPoints[fireIndex].transform).GetComponent<FoodItem>();
        
        foodInstance.transform.SetParent(null);
        foodInstance.timeToLive = 12f;
        foodInstance.speed = 3f;
        foodInstance.velocity = targetAngle;
        }

    private void ResetTimer()
    {
        spawnTimer = UnityEngine.Random.Range(_coolDownMin, _coolDownMax);
    }
}
