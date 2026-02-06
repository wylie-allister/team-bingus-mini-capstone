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
    [SerializeField] private float _enemySpawnRadius = 5.0f;

    [Header("Camp Clearing")]
    [SerializeField] private float _clearingRadius = 8.0f;

    [Header("Prop Offsets (LOCAL to camp)")]
    [SerializeField] private Vector3 _markedTreeLocalOffset = new Vector3(2f, 0f, 1f);
    [SerializeField] private Vector3 _loggingTruckLocalOffset = new Vector3(-2f, 0f, -1f);

    // Exposed read-only for WorldGenerator safe padding
    public float ClearingRadius => _clearingRadius;
    public float EnemySpawnRadius => _enemySpawnRadius;

    private readonly List<GameObject> _enemies = new();
    private GameObject _markedTreeObject;
    private GameObject _loggingTruckObject;

    private FoliageManager _foliageManager;

    // Bounds sent from WorldGenerator so we can clamp props/enemies
    private bool _hasBounds = false;
    private Bounds _groundBounds;

    public void SetGroundBounds(Bounds b)
    {
        _groundBounds = b;
        _hasBounds = true;
    }

    void Start()
    {
        if (_markedTreePrefab == null || _loggingTruckPrefab == null || _enemyPrefab == null)
        {
            Debug.LogError("Camp Prefabs are not set");
            return;
        }

        _foliageManager = FindObjectOfType<FoliageManager>();

        InitializePrefabs(_campEnemyCount);

        // Clear foliage around camp (human look)
        if (_foliageManager != null)
            _foliageManager.ClearArea(transform.position, _clearingRadius);

        // Place props + enemies safely
        UpdateCampPropPositions();
        UpdateEnemySpawnPositions();
    }

    void InitializePrefabs(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab);
            newEnemy.transform.parent = transform;
            _enemies.Add(newEnemy);
        }

        _markedTreeObject = Instantiate(_markedTreePrefab);
        _loggingTruckObject = Instantiate(_loggingTruckPrefab);

        _markedTreeObject.transform.parent = transform;
        _loggingTruckObject.transform.parent = transform;
    }

    void UpdateEnemySpawnPositions()
    {
        foreach (GameObject enemy in _enemies)
        {
            Vector2 r = Random.insideUnitCircle * _enemySpawnRadius;
            Vector3 pos = transform.position + new Vector3(r.x, 0f, r.y);

            pos = ClampToBounds(pos);

            enemy.transform.position = pos;
            enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }
    }

    void UpdateCampPropPositions()
    {
        // LOCAL offsets -> world positions
        Vector3 markedPos = transform.TransformPoint(_markedTreeLocalOffset);
        Vector3 truckPos = transform.TransformPoint(_loggingTruckLocalOffset);

        markedPos = ClampToBounds(markedPos);
        truckPos = ClampToBounds(truckPos);

        _markedTreeObject.transform.position = markedPos;
        _loggingTruckObject.transform.position = truckPos;

        _markedTreeObject.transform.rotation = transform.rotation;
        _loggingTruckObject.transform.rotation = transform.rotation;
    }

    private Vector3 ClampToBounds(Vector3 pos)
    {
        if (!_hasBounds) return pos;

        // keep within plane bounds (no edge leak)
        pos.x = Mathf.Clamp(pos.x, _groundBounds.min.x, _groundBounds.max.x);
        pos.z = Mathf.Clamp(pos.z, _groundBounds.min.z, _groundBounds.max.z);

        // keep y on surface (plane is flat)
        pos.y = _groundBounds.max.y;

        return pos;
    }
}
