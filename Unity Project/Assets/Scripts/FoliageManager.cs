using System.Collections.Generic;
using UnityEngine;

public class FoliageManager : MonoBehaviour
{
    [Header("Ground (Spawn Bounds)")]
    [SerializeField] private Renderer groundRenderer;
    [SerializeField] private float edgePadding = 1.0f;

    [Header("Foliage Prefabs")]
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject[] bushPrefabs;

    [Header("Counts")]
    [SerializeField] private int treeCount = 80;
    [SerializeField] private int bushCount = 60;

    [Header("Placement")]
    [SerializeField] private float minTreeSpacing = 2.0f;
    [SerializeField] private float minBushSpacing = 1.2f;

    [Header("Optional Obstacle Filtering")]
    [SerializeField] private LayerMask obstacleMask;     // e.g., Camp layer (optional)
    [SerializeField] private float obstacleCheckRadius = 0.8f;

    private readonly List<GameObject> spawnedFoliage = new List<GameObject>();

    public void GenerateFoliage()
    {
        if (groundRenderer == null)
        {
            Debug.LogError("FoliageManager: Ground Renderer not assigned.");
            return;
        }

        ClearAllFoliage();

        SpawnBatch(treePrefabs, treeCount, minTreeSpacing);
        SpawnBatch(bushPrefabs, bushCount, minBushSpacing);
    }

    public void ClearArea(Vector3 center, float radius)
    {
        float rSqr = radius * radius;

        for (int i = spawnedFoliage.Count - 1; i >= 0; i--)
        {
            GameObject obj = spawnedFoliage[i];
            if (obj == null)
            {
                spawnedFoliage.RemoveAt(i);
                continue;
            }

            Vector3 p = obj.transform.position;
            p.y = center.y;

            Vector3 c = center;
            c.y = center.y;

            if ((p - c).sqrMagnitude <= rSqr)
            {
                Destroy(obj);
                spawnedFoliage.RemoveAt(i);
            }
        }
    }

    public void ClearAllFoliage()
    {
        for (int i = spawnedFoliage.Count - 1; i >= 0; i--)
        {
            if (spawnedFoliage[i] != null)
                Destroy(spawnedFoliage[i]);
        }
        spawnedFoliage.Clear();
    }

    private void SpawnBatch(GameObject[] prefabs, int count, float minSpacing)
    {
        if (prefabs == null || prefabs.Length == 0) return;

        int safety = 0;
        int spawned = 0;

        while (spawned < count && safety < count * 60)
        {
            safety++;

            Vector3 pos = RandomPointOnGround();

            // (Optional) don't spawn on obstacles (like camps/rocks)
            if (obstacleMask.value != 0)
            {
                if (Physics.CheckSphere(pos + Vector3.up * 0.25f, obstacleCheckRadius, obstacleMask))
                    continue;
            }

            // spacing check vs other foliage
            if (!HasSpacing(pos, minSpacing))
                continue;

            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject instance = Instantiate(prefab, pos, RandomYaw());
            instance.transform.parent = transform;
            spawnedFoliage.Add(instance);

            spawned++;
        }
    }

    private bool HasSpacing(Vector3 candidate, float minSpacing)
    {
        float msSqr = minSpacing * minSpacing;

        for (int i = 0; i < spawnedFoliage.Count; i++)
        {
            if (spawnedFoliage[i] == null) continue;

            Vector3 p = spawnedFoliage[i].transform.position;
            p.y = candidate.y;

            if ((p - candidate).sqrMagnitude < msSqr)
                return false;
        }

        return true;
    }

    private Vector3 RandomPointOnGround()
    {
        Bounds b = groundRenderer.bounds;

        float x = Random.Range(b.min.x + edgePadding, b.max.x - edgePadding);
        float z = Random.Range(b.min.z + edgePadding, b.max.z - edgePadding);

        // Plane is flat, so y can just be its top surface
        float y = b.max.y;

        return new Vector3(x, y, z);
    }

    private Quaternion RandomYaw()
    {
        return Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }
}
  