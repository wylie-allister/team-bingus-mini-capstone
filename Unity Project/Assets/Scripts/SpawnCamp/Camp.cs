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
    [SerializeField] private float _treeRemovalRadius = 9.0f;
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

        
        //------------Make this work
        //RemoveTreesNearCamp(GameManager.Instance.terrainGenerator.trees);
    }

    void InitializePrefabs(int enemyCount)
    {
        // Loop through enemy count, create new enemy and add to _enemies list
        for (int i = 0; i < enemyCount; i++)
        {
           GameObject newEnemy = GameObject.Instantiate(_enemyPrefab, this.transform);
           newEnemy.transform.position = this.transform.position;
           _enemies.Add(newEnemy);
        }

        // Instantiate other prefabs for camp
        _markedTreeObject = GameObject.Instantiate(_markedTreePrefab, this.transform);
        _markedTreeObject.transform.position = this.transform.position + _markedTreePositionOffset;
        
        _loggingTruckObject = GameObject.Instantiate(_loggingTruckPrefab, this.transform);
        _loggingTruckObject.transform.position = this.transform.position + _loggingTruckPositionOffset;
        
        
        // Set instantiated prefab parents to Camp object
    }

    void UpdateEnemySpawnPositions()
    {
        // Loop through all enemies for this camp
        foreach (GameObject enemy in _enemies)
        {
            //Debug.Log($"ET: {enemy.transform.position.x}, {enemy.transform.position.y}, {enemy.transform.position.z}");
            // Create temp transform where the given enemy is located
            //Transform tempTransform = enemy.transform;
            
            // Random rotation in the y-axis
            enemy.transform.Rotate(0f, Random.Range(0.0f, 360.0f), 0f);
            
            // Position temp transform forward along the previously calc'd angle
            // multiplied by a random range, extending to the given spawn radius for this camp
            enemy.transform.position += enemy.transform.forward * Random.Range(2f, _enemySpawnRadius);
            
            // Set enemy position to temp transform position
            //enemy.transform.position = enemy.transform.position;
        }
    }

    
    void Update()
    {
        // Apply position offsets to given objects
        
    }

    void RemoveTreesNearCamp(List<GameObject> trees)
    {
        for (int i = 0; i < trees.Count; i++)
        {
            if (TestFunc(trees[i].transform.position.x, trees[i].transform.position.y,
                this.transform.position.x - _treeRemovalRadius, this.transform.position.x + _treeRemovalRadius,
                this.transform.position.y - _treeRemovalRadius, this.transform.position.y + _treeRemovalRadius))//(IsPositionWithinRadius(trees[i].transform.position, this.transform.position, _treeRemovalRadius))
            {
                Destroy(trees[i].gameObject);
                trees.RemoveAt(i);
            }
        }
    }

    // Test radius func -tbd/m
    bool TestFunc(float pX, float pY, float minX, float maxX, float minY, float maxY)
    {
        bool withinX = pX >= minX && pX <= maxX;
        bool withinY = pY >= minY && pY <= maxY;
        
        return withinX && withinY;
    }
    
    bool IsPositionWithinRadius(Vector3 position, Vector3 point, float radius)
    {
        if (position.x > point.x - radius && position.x < point.x + radius)
            if (position.y > point.y - radius && position.y < point.y + radius)
                return true;

        return false;
    }

    public void RemoveSmoke()
    {
        //when a mark is scratched, remove smoke from loggingtruckprefab. It is late at night and I am too stupid to figure out how to do this right now,
        //so this is being left here as a reminder to do it
    }
}
