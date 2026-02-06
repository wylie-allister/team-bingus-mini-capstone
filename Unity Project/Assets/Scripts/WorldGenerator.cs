using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FoliageManager foliageManager;

    [Header("Ground (Spawn Bounds)")]
    [SerializeField] private Renderer groundRenderer;

    [Header("Camp Spawning")]
    [SerializeField] private GameObject campPrefab;
    [SerializeField] private int campCount = 3;

    [Tooltip("Minimum distance between camp centers.")]
    [SerializeField] private float minCampSpacing = 18f;

    [Tooltip("How many attempts per camp before giving up.")]
    [SerializeField] private int maxAttemptsPerCamp = 80;

    [Tooltip("Extra padding beyond camp clearing/enemy radius.")]
    [SerializeField] private float extraEdgePadding = 1.0f;

    private readonly List<Vector3> spawnedCampPositions = new();

    void Start()
    {
        if (foliageManager == null)
            foliageManager = FindObjectOfType<FoliageManager>();

        if (groundRenderer == null)
        {
            Debug.LogError("WorldGenerator: Ground Renderer not assigned. Drag the Plane into the field.");
            return;
        }

        // 1) Generate foliage first (so camps can clear it)
        if (foliageManager != null)
            foliageManager.GenerateFoliage();

        // 2) Spawn camps with spacing + safe margins
        SpawnCamps();
    }

    private void SpawnCamps()
    {
        if (campPrefab == null)
        {
            Debug.LogError("WorldGenerator: Camp prefab not assigned.");
            return;
        }

        for (int i = 0; i < campCount; i++)
        {
            bool placed = TryFindCampPosition(out Vector3 pos);

            if (!placed)
            {
                Debug.LogWarning($"WorldGenerator: Could not place camp #{i} (increase plane size, reduce campCount, or reduce spacing).");
                continue;
            }

            spawnedCampPositions.Add(pos);

            GameObject campObj = Instantiate(campPrefab, pos, Quaternion.identity);

            // (Optional) pass bounds to camp (so camp can clamp enemies/props)
            Camp camp = campObj.GetComponent<Camp>();
            if (camp != null)
                camp.SetGroundBounds(groundRenderer.bounds);
        }
    }

    private bool TryFindCampPosition(out Vector3 result)
    {
        Bounds b = groundRenderer.bounds;

        // Compute safe edge padding based on camp settings
        float safePadding = extraEdgePadding;

        Camp campOnPrefab = campPrefab.GetComponent<Camp>();
        if (campOnPrefab != null)
        {
            // IMPORTANT: these getters are in the updated Camp script
            safePadding += campOnPrefab.ClearingRadius;
            safePadding = Mathf.Max(safePadding, campOnPrefab.ClearingRadius + campOnPrefab.EnemySpawnRadius + extraEdgePadding);
        }
        else
        {
            // fallback if no Camp component (still keep some edge padding)
            safePadding += 4f;
        }

        for (int attempt = 0; attempt < maxAttemptsPerCamp; attempt++)
        {
            float x = Random.Range(b.min.x + safePadding, b.max.x - safePadding);
            float z = Random.Range(b.min.z + safePadding, b.max.z - safePadding);
            Vector3 candidate = new Vector3(x, b.max.y, z);

            if (IsFarEnoughFromOtherCamps(candidate))
            {
                result = candidate;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    private bool IsFarEnoughFromOtherCamps(Vector3 candidate)
    {
        float minSqr = minCampSpacing * minCampSpacing;

        for (int i = 0; i < spawnedCampPositions.Count; i++)
        {
            Vector3 p = spawnedCampPositions[i];
            p.y = candidate.y;

            if ((p - candidate).sqrMagnitude < minSqr)
                return false;
        }

        return true;
    }
}
