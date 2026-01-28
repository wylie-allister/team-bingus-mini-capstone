using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _markedTreePrefab;
    [SerializeField] private GameObject _loggingTruckPrefab;
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Enemies")]
    [SerializeField] private int _campEnemyCount = 4;
    // Swap this to global enemy if necessary
    private List<GameObject> _enemies = new List<GameObject>();
    
    
    private GameObject _markedTreeObject;
    private GameObject _loggingTruckObject;

    [Header("Position Offsets")]
    [SerializeField] private float _enemySpawnRadius = 5.0f;
    [SerializeField] private Vector3 _markedTreePositionOffset = new Vector3();
    [SerializeField] private Vector3 _loggingTruckPositionOffset = new Vector3();
    

    void Start()
    {
        // If prefabs are missing from the gameobject, throw error
        if (_markedTreePrefab == null || _loggingTruckPrefab == null || _enemyPrefab == null)
        {
            Debug.LogError("Camp Prefabs are not set");
        }

        // Initialize prefabs, requires enemy count from inspector
        InitializePrefabs(_campEnemyCount);
        
        // Update enemy spawn positions
        UpdateEnemySpawnPositions();
    }

    void InitializePrefabs(int enemyCount)
    {
        // Loop through enemy count, create new enemy and add to _enemies list
        for (int i = 0; i < enemyCount; i++)
        {
           GameObject newEnemy = GameObject.Instantiate(_enemyPrefab);
           newEnemy.transform.parent = transform;
           _enemies.Add(newEnemy);
        }

        // Instantiate other prefabs for camp
        _markedTreeObject = GameObject.Instantiate(_markedTreePrefab);
        _loggingTruckObject = GameObject.Instantiate(_loggingTruckPrefab);
        
        // Set instantiated prefab parents to Camp object
        _markedTreeObject.transform.parent = transform;
        _loggingTruckObject.transform.parent = transform;
    }

    void UpdateEnemySpawnPositions()
    {
        // Loop through all enemies for this camp
        foreach (GameObject enemy in _enemies)
        {
            // Create temp transform where the given enemy is located
            Transform tempTransform = enemy.transform;
            
            // Random rotation in the y-axis
            tempTransform.Rotate(0f, Random.Range(0.0f, 360.0f), 0f);
            
            // Position temp transform forward along the previously calc'd angle
            // multiplied by a random range, extending to the given spawn radius for this camp
            tempTransform.position = tempTransform.forward * Random.Range(0.1f, _enemySpawnRadius);
            
            // Set enemy position to temp transform position
            enemy.transform.position = tempTransform.position;
        }
    }

    
    void Update()
    {
        // Apply position offsets to given objects
        _markedTreeObject.transform.position = _markedTreePositionOffset;
        _loggingTruckObject.transform.position = _loggingTruckPositionOffset;
    }
}
