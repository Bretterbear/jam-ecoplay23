using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDirector : MonoBehaviour
{
    // --- Serialized Variable Declarations --- //
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float[] spawnTime;
    [Tooltip("in seconds - default 300")]
    [SerializeField] private float levelLength = 300f;

    // --- Serialized Variable Declarations --- //
    private float gameTimer;
    private bool[] _bHasSpawned;
    private int _currentSpawnIndex;
    private int arrayLength;

    void Start()
    {
        gameTimer = 0;
        _currentSpawnIndex = 0;
        arrayLength = enemyPrefabs.Length;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            _bHasSpawned[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime;

        if (_currentSpawnIndex < arrayLength)
        {
            if (gameTimer > spawnTime[_currentSpawnIndex])
            {
                Instantiate(enemyPrefabs[_currentSpawnIndex]);
                _currentSpawnIndex++;
            }
        }

        if (gameTimer > levelLength)
        {
            Debug.Log("YOU WIN!");
        }
    }


}
