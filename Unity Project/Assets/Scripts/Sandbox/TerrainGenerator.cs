using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Model References")]
    [SerializeField] private GameObject[] treePrefabs;

    [Header("Settings")] 
    public Transform treeParent;
    public int numberOfTrees;

    public Vector2 treeSpawnRadius;
    public List<GameObject> trees = new List<GameObject>();
    public float treeSpacingOffset = 2.0f;
    
    [Header("Camps")] 
    public GameObject campPrefab;
    List<GameObject> camps = new List<GameObject>();
    public GameObject campParent;
    public int campCount = 5;
    
    [Header("Grass")]
    public GameObject grassPrefab;

    public int grassInstanceCount = 0;
    


    void Awake()
    {
        HandleTreeRandomization();
    }
    
    void Start()
    {
        SpawnCamps();

    }

    void Update()
    {
    }

    public void HandleTreeRandomization()
    {
        // Create new list of trees with an individual random position
        for (int treeIndex = 0; treeIndex < numberOfTrees; treeIndex++)
        {
            GameObject newTree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)]);

            RandomizeTreePosition(newTree);
            newTree.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.8f, 1.2f);
            newTree.transform.parent = treeParent;
            trees.Add(newTree);
        }
    }

    public void RandomizeTreePosition(GameObject tree)
    {
        // Get new random position within the spawn radius
        Vector3 randomPosition = new Vector3(Random.Range(-treeSpawnRadius.x, treeSpawnRadius.x), 3, Random.Range(-treeSpawnRadius.y, treeSpawnRadius.y));
        RaycastHit hit;
        if (Physics.Raycast(randomPosition, Vector3.down, out hit, 5f, LayerMask.GetMask("Ground")))
        {
            randomPosition.y = hit.point.y - 0.5f;
        }
        
        // Set tree position to new random position
        tree.transform.position = randomPosition;
        tree.transform.SetParent(treeParent);
    }

    public void SpawnCamps()
    {
        // Camp spawn radius lies within the tree spawn radius
        Vector2 campSpawnRadius = new Vector2(treeSpawnRadius.x- 5.0f, treeSpawnRadius.y - 5.0f);
        
        // Create new camps and append to list
        for (int i = 0; i < camps.Count; i++)
        {
            GameObject newCamp = Instantiate(campPrefab);
            newCamp.transform.position = new Vector3(Random.Range(-campSpawnRadius.x, campSpawnRadius.y), 0, Random.Range(-campSpawnRadius.y, campSpawnRadius.y));
            //newCamp.transform.parent = campParent.transform;
            
            camps.Add(newCamp);
        }
    }
}
