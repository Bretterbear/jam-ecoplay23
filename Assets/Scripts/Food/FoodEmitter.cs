using Services;
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
    private PoolService _poolService;

    private void Start()
    {
        _poolService = ServiceLocator.Instance.Get<PoolService>();
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
            ResetTimer();
            FireFood();
        }
    }

    private void FireFood()
    {
        int fireIndex = Random.Range(0, controlPointCount);
        Vector3 targetVector = makeTargetingVector(fireIndex);

        GameObject foodInstance = _poolService.GetProjectileFromPoop(ProjectileType.Food_Seaweed);
        if (foodInstance == null)
        {
            foodInstance = MakeNewFood(ProjectileType.Food_Seaweed, fireIndex);
        }

        FoodItem foodBlast = foodInstance.GetComponent<FoodItem>();

        float twistAngle = Mathf.Atan2(targetVector.y, targetVector.x);
        float twistDegrees = twistAngle * Mathf.Rad2Deg;

        foodBlast.transform.position = FoodSpawnPoints[fireIndex].transform.position;
        foodBlast.timeToLive = 12f;
        foodBlast.speed = 3f;
        foodBlast.rotation = twistDegrees;
        foodBlast.velocity = Vector2.right;
    }

    private Vector3 makeTargetingVector(int fireIndex)
    {

        float xRand = Random.Range(-_xSpread, _xSpread);
        float yRand = Random.Range(-_ySpread, _ySpread);
        Vector3 targetAngle = new Vector3(xRand, yRand, 0);

        targetAngle = (targetAngle - FoodSpawnPoints[fireIndex].transform.position).normalized;
        return targetAngle;
    }

    private void ResetTimer()
    {
        spawnTimer = UnityEngine.Random.Range(_coolDownMin, _coolDownMax);
    }

    /// <summary>
    /// Instantiates a new food and returns it; might be unnecessary as a func
    /// </summary>
    /// <param name="style">style of food to be instatiated</param>
    /// <returns>new food</returns>
    public GameObject MakeNewFood(ProjectileType style, int fireIndex)
    {
        GameObject newFood = Instantiate(FoodSource, FoodSpawnPoints[fireIndex].transform);

        return newFood;
    }
}
